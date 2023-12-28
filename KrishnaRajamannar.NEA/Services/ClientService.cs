using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System.Windows;
using KrishnaRajamannar.NEA.Events;
using System.Diagnostics;
using KrishnaRajamannar.NEA.ViewModels.Dto;
using System.Text.Json;

namespace KrishnaRajamannar.NEA.Services
{
    public class ClientService : IClientService
    {
        //Create a event handler 
        public event ClientConnectedEventHandler ClientConnected;
        private readonly string sessionId;
        private TcpClient server = new TcpClient();

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            ClientConnectedEventHandler handler = ClientConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public string ConnectToServer(string username, int userId, string ipAddressConnect, int portNumberConnect, string sessionId)
        {
            string messageFromServer = "";

            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    messageFromServer = HandleClientRequests(username,userId, ipAddressConnect, portNumberConnect, sessionId);
                });

                task.Wait();
                return messageFromServer;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return messageFromServer;
        }

        public string HandleClientRequests(string username, int userID, string ipAddressConnect, int portNumberConnect, string sessionId)
        {
            var buffer = new byte[1024];
            string messageFromServer = "";
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddressConnect), portNumberConnect);
            server.Connect(endPoint);

            //Raise the Event 
            OnClientConnected(new ClientConnectedEventArgs() { SessionId = sessionId });

            NetworkStream stream = server.GetStream();
            UserSessionDto dto = new UserSessionDto
            {
                SessionId = sessionId,
                Name = username,
                Id = userID
            };

            var payload = JsonSerializer.Serialize<UserSessionDto>(dto);

            var messageBytes = Encoding.UTF8.GetBytes(payload);

            stream.Write(messageBytes, 0, messageBytes.Length);

            Task.Delay(1000).Wait();

            while (true)
            {
                if (stream.DataAvailable == true)
                {
                    var reading = stream.Read(buffer, 0, buffer.Length);
                    messageFromServer = Encoding.UTF8.GetString(buffer, 0, reading);
                    Debug.Print(messageFromServer);
                }

            }
        }
    }
}
