using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Models.DTO;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services.Connection
{
    // Handles the client side for the connection between the host/server and the client
    public class ClientService : IClientService
    {
        public event ClientConnectedEventHandler ClientConnected;
        public event StartQuizEventHandler StartQuizEvent;
        public event EndQuizEventHandler EndQuizEvent;
        public event ProcessServerResponseEventHandler ProcessServerResponse;

        // Used to notify the server about which session a client will connect to  
        private string sessionID;
        private TcpClient client = new TcpClient();
        // Uses this to temporarily store the responses from the server
        // Which will then be dequeued and processed
        private ConcurrentQueue<ServerResponse> serverResponses = new ConcurrentQueue<ServerResponse>();

        public ClientService() {

            // Creates a separate thread outside of the main thread to listen for responses from the server
            Thread workerThread = new Thread(() =>
            {
                ListenAndProcessServerResponses();
            });
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();

        }

        // Processes all the different data types for messages from the server
        private void ListenAndProcessServerResponses()
        {
            while (true)
            {
                try 
                {
                    // Checks if serverResponses queue is not empty 
                    if (!serverResponses.IsEmpty) 
                    {
                        ServerResponse response = null;
                        // Dequeues the last response from the server
                        serverResponses.TryDequeue(out response);
                        if (response != null)
                        {
                            switch (response.DataType)
                            {
                                // This case is used when the server sends a response back to the client
                                // to notify them of the successful connection
                                case "Acknowledgement":
                                    ClientConnectedEventArgs args = new ClientConnectedEventArgs();
                                    args.ServerResponse = response;
                                    OnClientConnected(args);
                                    break;
                                // This case is used when the server sends a message to start the review of the quiz
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
                                    // This is used to process general responses
                                    // which is everything else with the correct answer being sent from the server as an example
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

        // An event for when the client is first connected to the session
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
                // Used to connect to the server on a separate thread
                Task task = Task.Factory.StartNew(() =>
                {
                    // Used to connect to the server and read the response from the server about the connection
                    messageFromServer = HandleClientRequests(username, userId, ipAddressConnect, portNumberConnect);
                });

                return messageFromServer;

            }
            catch (Exception ex)
            {
                ;
            }
            return messageFromServer;
        }
        // Used to connect to the server and read the response from the server about the connection
        public string HandleClientRequests(string username, int userID, string ipAddressConnect, int portNumberConnect)
        {
            var buffer = new byte[4096];
            string messageFromServer = "";
            // Defines the network endpoint which the client is trying to connect to 
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddressConnect), portNumberConnect);
            client.Connect(endPoint);
                     
            NetworkStream stream = client.GetStream();
            // Used to group the data about the user who has just connected
            UserSessionData userData = new UserSessionData
            {
                SessionID = sessionID,
                Username = username,
                UserID = userID
            };

            // Serialises the object
            var payload = JsonSerializer.Serialize(userData);
            var messageBytes = Encoding.UTF8.GetBytes(payload);
            // Sends the message down the stream to the server
            // The message represents 
            stream.Write(messageBytes, 0, messageBytes.Length);
            // This is used to prevent the server from the reading responses before a response from the client is sent
            Task.Delay(1000).Wait();

            while (true)
            {
                // Checks if new data has been passed through the stream
                if (stream.DataAvailable == true)
                {
                    // Used to recieve the response from the server about the connection
                    var reading = stream.Read(buffer, 0, buffer.Length);
                    messageFromServer = Encoding.UTF8.GetString(buffer, 0, reading);
                    // Deserialises to receive the response object from the server               
                    ServerResponse response = JsonSerializer.Deserialize<ServerResponse>(messageFromServer);
                    // Enqueues the response so that it can be processed
                    serverResponses.Enqueue(response);
                }

            }
        }
        // This is used to send general data to the server.
        public void SendDataToServer(string data, string dataType, int userID, string username, int totalPoints) 
        {
            NetworkStream stream = client.GetStream();
            // Used to notify the server about the details of the client sending the data
            UserSessionData userData = new UserSessionData
            {
                SessionID = sessionID,
                Username = username,
                UserID = userID,
                TotalPoints = totalPoints
            };
            var userDataPayload = JsonSerializer.Serialize(userData);
            // userDataPayload is then assigned to user data in the client response
            ClientResponse clientResponse = new ClientResponse
            {
                SessionID = sessionID,
                DataType = dataType,
                Data = data,
                UserData = userDataPayload
            };
            // The ClientResponse object is then serialised
            var payload = JsonSerializer.Serialize(clientResponse);
            var messageBytes = Encoding.UTF8.GetBytes(payload);
            // Data sent down the stream to the server
            stream.Write(messageBytes, 0, messageBytes.Length);
            // This is used to prevent the server from the reading responses before a response from the client is sent
            Task.Delay(1000).Wait();
        }
    }
}
