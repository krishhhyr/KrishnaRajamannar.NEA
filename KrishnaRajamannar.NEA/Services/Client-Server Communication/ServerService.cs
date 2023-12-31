﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Views;
//using log4net;
using Microsoft.Identity.Client;
using System.IO.Packaging;
using System.Collections.Concurrent;
using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models.DTO;
using Azure;
using System.Text.Json.Serialization;
using System.Runtime.Serialization.Json;

namespace KrishnaRajamannar.NEA.Services.Connection
{
    public class ServerService : IServerService
    {
        public event ProcessClientResponseEventHandler ProcessClientResponse;

        TcpListener server;
        private string _hostname;
        private List<TcpClient> clients = new List<TcpClient>();
        CancellationTokenSource source = new CancellationTokenSource();
        UserConnectionService _userConnectionService;
        IUserSessionService _userSessionService;
        ISessionService _sessionService;

        private UserSessionData userSessionData;
        private ConcurrentQueue<ClientResponse> clientResponses = new ConcurrentQueue<ClientResponse>();

        public ServerService(UserConnectionService userConnectionService, IUserSessionService userSessionService, ISessionService sessionService)
        {
            _userConnectionService = userConnectionService;
            _userSessionService = userSessionService;
            _sessionService = sessionService;

            Thread workerThread = new Thread(() =>
            {
                ListenAndProcessClientResponses();
            });
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();
        }

        public void StartServer(string hostname, string ipAddress, int portNumber)
        {
            _hostname = hostname;
            ListeningForConnections(ipAddress, portNumber);
        }

        public void ListenAndProcessClientResponses()
        {
            while (true)
            {
                try
                {
                    RecieveDataFromClients();

                    if (!clientResponses.IsEmpty)
                    {
                        ClientResponse clientResponse = null;
                        clientResponses.TryDequeue(out clientResponse);
                        if (clientResponse != null)
                        {
                            ProcessClientResponseEventArgs args = new ProcessClientResponseEventArgs();
                            args.ClientResponse = clientResponse;
                            OnProcessClientResponse(args);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
            }

        }

        protected virtual void OnProcessClientResponse(ProcessClientResponseEventArgs e)
        {
            ProcessClientResponseEventHandler handler = ProcessClientResponse;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void ListeningForConnections(string ipAddress, int portNumber)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), portNumber);

            server = new TcpListener(endPoint);
            // Creates a new thread to listen and accept client connections
            // Used to allow UI thread to update simultaneously
            Task task = Task.Factory.StartNew(() =>
            {
                server.Start();
                while (true)
                {
                    HandleClientRequests(server);
                }
            }, source.Token);

        }
        public void HandleClientRequests(TcpListener server)
        {
            byte[] buffer = new byte[1024];
            TcpClient client = server.AcceptTcpClient();
            // Adds client to list to keep record of all connected clients
            clients.Add(client);
            NetworkStream stream = client.GetStream();
            // Reads the data that the client has sent when first connecting
            var reading = stream.Read(buffer, 0, buffer.Length);
            string usersession = Encoding.UTF8.GetString(buffer, 0, reading);
            // Converts the string into an object
            // Object represents the UserID and Username of client connected
            userSessionData = JsonSerializer.Deserialize<UserSessionData>(usersession);
            // Calls a series of events to update UI to reflect new client connection
            _userConnectionService.UserJoinedSession(userSessionData);
            // Calls function to respond to new client connection
            SendAcknowledgement(client);
        }

        // Sends a response message back to clients after inital connection
        // (acknowledges the connection, signals clients that a connection has been established)
        // Used to display same data that the clients can see in a different window
        public void SendAcknowledgement(TcpClient client)
        {
            ServerResponse serverResponse = new ServerResponse();
            SessionData sessionData = new SessionData();

            serverResponse.SessionId = userSessionData.SessionID;
            // Gets the quiz that was selected by the host when starting the session
            sessionData = _sessionService.GetSessionData(Convert.ToInt32(userSessionData.SessionID));
            // Gets the details of the users connected to the session
            // Used to pass onto the client's UI window 
            IList<UserSessionData> userDetails = _userSessionService.GetUserSessionDetails(userSessionData);
            // Creates a new object to pass the selected Quiz, the other users connected to client
            // and host name

            sessionData.HostName = _hostname;
            sessionData.UserSessions = userDetails;
            // Expressing the type of data so that the Client can process the request accordingly
            serverResponse.DataType = "Acknowledgement";
            // Serialising all the whole sessiondata object (i.e selected quiz and users)
            serverResponse.Data = JsonSerializer.Serialize<SessionData>(sessionData);
            NetworkStream stream = client.GetStream();
            // Serialising the whole server response message and writing that in the stream with client
            // Full server response is session ID, data and dataType
            // Within data, it's the quiz selected by host and a list of the details of other users who
            // have joined the session (UserSessionDto object)
            var payload = JsonSerializer.Serialize<ServerResponse>(serverResponse);
            var messageBytes = Encoding.UTF8.GetBytes(payload);
            stream.Write(messageBytes, 0, messageBytes.Length);

        }

        // Used to send general messages to all clients connected
        public void SendDataToClients(string data, string dataType)
        {
            foreach (var client in clients)
            {
                ServerResponse serverResponse = new ServerResponse();
                serverResponse.SessionId = userSessionData.SessionID;
                serverResponse.DataType = dataType;
                serverResponse.Data = data;
                NetworkStream stream = client.GetStream();

                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.WriteIndented = false;

                var payload = JsonSerializer.Serialize<ServerResponse>(serverResponse, jsonSerializerOptions);
                var messageBytes = Encoding.UTF8.GetBytes(payload);
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private void RecieveDataFromClients() 
        {
            var buffer = new byte[4096];
            string messageFromClient = null;

            foreach (var client in clients) 
            {
                NetworkStream stream = client.GetStream();
                
                if (stream.DataAvailable == true) 
                {
                    var reading = stream.Read(buffer, 0, buffer.Length);
                    messageFromClient = Encoding.UTF8.GetString(buffer, 0, reading);
                    ClientResponse clientResponse = new ClientResponse();
                    clientResponse = JsonSerializer.Deserialize<ClientResponse>(messageFromClient);
                    clientResponses.Enqueue(clientResponse);
                }
            }
        }

        public void StopServer()
        {
            foreach (var client in clients) 
            {
                if (client.Connected == true) 
                {
                    client.Close();
                }
            }


            server.Stop();
            source.Cancel();
            //source.Dispose();
        }
    }
}
