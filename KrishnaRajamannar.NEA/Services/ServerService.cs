using System;
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
using KrishnaRajamannar.NEA.ViewModels.Dto;
using KrishnaRajamannar.NEA.Views;
using log4net;
using Microsoft.Identity.Client;

namespace KrishnaRajamannar.NEA.Services
{
    public class ServerService :  IServerService 
    {
        private List<TcpClient> _clients = new List<TcpClient>();
        private static readonly ILog log = LogManager.GetLogger(typeof(ServerService));

        TcpListener server;

        CancellationTokenSource source = new CancellationTokenSource();

        UserConnectionService _userConnectionService;
        IUserSessionService _userSessionService;
        ISessionService _sessionService;

        public ServerService(UserConnectionService userConnectionService, IUserSessionService userSessionService, ISessionService sessionService)
        {
            _userConnectionService = userConnectionService;
            _userSessionService = userSessionService;
            _sessionService = sessionService;
        }

        public void StartServer(string ipAddress, int portNumber) 
        {
            ListeningForConnections(ipAddress, portNumber);
        }
        public void ListeningForConnections(string ipAddress, int portNumber) 
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), portNumber);

            server = new TcpListener(endPoint);
            Task task = Task.Factory.StartNew(() => 
            {
                server.Start();
                while (true) 
                {
                    HandleClientRequests(server);
                }
            } ,source.Token);
            
        }
        public void HandleClientRequests(TcpListener server) 
        {
            byte[] buffer = new byte[1024];
            TcpClient client = server.AcceptTcpClient();            
            _clients.Add(client);
            NetworkStream stream = client.GetStream();
            var reading = stream.Read(buffer, 0, buffer.Length);
            string usersession = Encoding.UTF8.GetString(buffer, 0, reading);
            UserSessionDto userSessionDto = JsonSerializer.Deserialize<UserSessionDto>(usersession);
            _userConnectionService.UserJoinedSession(userSessionDto);
            SendAcknowledgement(client, userSessionDto);
        }

        public void StopServer() 
        {
            server.Stop();
            source.Cancel();
            source.Dispose();
        }

        public void SendCommandToClients(string command)
        {
            foreach (var client in _clients) {
                NetworkStream stream = client.GetStream();
                var messageBytes = Encoding.UTF8.GetBytes(command);
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        public void SendAcknowledgement(TcpClient client, UserSessionDto dto)
        {
            SessionAcknowledement sessionAcknowledement = new SessionAcknowledement();
            sessionAcknowledement.SessionId = dto.SessionId;

            var quizSelected = _sessionService.GetQuizSelectedForSession(Convert.ToInt32(dto.SessionId));

            // Get users for the session
            _userSessionService.GetUserSessionDetails(dto);

            sessionAcknowledement.QuizSelected = quizSelected;

            NetworkStream stream = client.GetStream();

            var payload = JsonSerializer.Serialize<SessionAcknowledement>(sessionAcknowledement);

            var messageBytes = Encoding.UTF8.GetBytes(payload);
            
            stream.Write(messageBytes, 0, messageBytes.Length);
           
        }
    }
}
