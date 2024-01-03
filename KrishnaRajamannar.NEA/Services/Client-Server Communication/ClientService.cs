using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Models.DTO;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services.Connection
{
    public class ClientService : IClientService
    {
        //Create a event handler 
        public event ClientConnectedEventHandler ClientConnected;
        public event StartQuizEventHandler StartQuizEvent;
        public event EndQuizEventHandler EndQuizEvent;
        public event ProcessServerResponseEventHandler ProcessServerResponse;

        private string sessionID;
        private TcpClient client = new TcpClient();
        private ConcurrentQueue<ServerResponse> serverResponses = new ConcurrentQueue<ServerResponse>();

        public ClientService() {

            Thread workerThread = new Thread(() =>
            {
                StartWorkerThread();
            });
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();

        }

        // Processes all the different data types for messages...
        private void StartWorkerThread()
        {
            while (true)
            {
                try 
                {
                    if (!serverResponses.IsEmpty) 
                    {
                        ServerResponse response = null;
                        serverResponses.TryDequeue(out response);
                        if (response != null)
                        {
                            Debug.Print(response.DataType);
                            switch (response.DataType)
                            {
                                case "Acknowledgement":
                                    ClientConnectedEventArgs args = new ClientConnectedEventArgs();
                                    args.ServerResponse = response;
                                    OnClientConnected(args);
                                    break;
                                case "StartQuiz":
                                    // We pass usual quiz data; I.e time limit or number of qs?
                                    // subscribe to event in ViewSessionInfo + Host Session Window??
                                    StartQuizEventArgs argsStartQuiz = new StartQuizEventArgs();
                                    argsStartQuiz.ServerResponse = response;
                                    OnStartQuizButtonPressed(argsStartQuiz);
                                    break;
                                case "EndQuiz":
                                    EndQuizEventArgs argsEndQuiz = new EndQuizEventArgs();
                                    argsEndQuiz.ServerResponse = response;
                                    OnEndQuiz(argsEndQuiz);
                                    break;
                                default:
                                    ProcessServerResponseEventArgs argsProcessServerResponse = new ProcessServerResponseEventArgs();
                                    argsProcessServerResponse.ServerResponse = response;
                                    OnProcessServerResponse(argsProcessServerResponse);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    ;
                }
            }
        }

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            ClientConnectedEventHandler handler = ClientConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnStartQuizButtonPressed(StartQuizEventArgs e)
        {
            StartQuizEventHandler handler = StartQuizEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnProcessServerResponse(ProcessServerResponseEventArgs e)
        {
            ProcessServerResponseEventHandler handler = ProcessServerResponse;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnEndQuiz(EndQuizEventArgs e) 
        {
            EndQuizEventHandler handler = EndQuizEvent;
            if (handler != null) 
            {
                handler(this, e);
            }
        }


        public string ConnectToServer(string username, int userId, string ipAddressConnect, int portNumberConnect, string sessionId)
        {
            string messageFromServer = "";
            sessionID = sessionId;

            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    messageFromServer = HandleClientRequests(username, userId, ipAddressConnect, portNumberConnect);
                });

                //task.Wait();
                return messageFromServer;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return messageFromServer;
        }

        public string HandleClientRequests(string username, int userID, string ipAddressConnect, int portNumberConnect)
        {
            var buffer = new byte[4096];
            string messageFromServer = "";
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddressConnect), portNumberConnect);
            client.Connect(endPoint);
                     

            NetworkStream stream = client.GetStream();
            UserSessionData userData = new UserSessionData
            {
                SessionID = sessionID,
                Username = username,
                UserID = userID
            };

            var payload = JsonSerializer.Serialize(userData);

            var messageBytes = Encoding.UTF8.GetBytes(payload);

            stream.Write(messageBytes, 0, messageBytes.Length);

            Task.Delay(1000).Wait();

            while (true)
            {
                if (stream.DataAvailable == true)
                {
                    var reading = stream.Read(buffer, 0, buffer.Length);
                    messageFromServer = Encoding.UTF8.GetString(buffer, 0, reading);
                    //Pass the recieved message to queue for further processing                    
                    ServerResponse response = JsonSerializer.Deserialize<ServerResponse>(messageFromServer);
                    serverResponses.Enqueue(response);
                }

            }
        }
        // would most likely need a client response as well?
        public void SendDataToServer(string data, string dataType, int userID, string username, int totalPoints) 
        {
            NetworkStream stream = client.GetStream();
            UserSessionData userData = new UserSessionData
            {
                SessionID = sessionID,
                Username = username,
                UserID = userID,
                TotalPoints = totalPoints
            };
            var userDataPayload = JsonSerializer.Serialize(userData);

            ClientResponse clientResponse = new ClientResponse
            {
                SessionID = sessionID,
                DataType = dataType,
                Data = data,
                UserData = userDataPayload
            };

            var payload = JsonSerializer.Serialize(clientResponse);
            var messageBytes = Encoding.UTF8.GetBytes(payload);

            stream.Write(messageBytes, 0, messageBytes.Length);

            Task.Delay(1000).Wait();
        }
    }
}
