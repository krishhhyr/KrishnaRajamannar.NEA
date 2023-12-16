using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KrishnaRajamannar.NEA.Services.Interfaces;
using Microsoft.Identity.Client;

namespace KrishnaRajamannar.NEA.Services
{
    public class ServerService : IServerService 
    {
        TcpListener server;

        CancellationTokenSource source = new CancellationTokenSource();

        public ServerService() 
        {
            
        }

        public void StartServer(string ipAddress, int portNumber) 
        {
            Task task = Task.Factory.StartNew(() =>
            {
                ListeningForConnections(ipAddress, portNumber);
            });
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
            using TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            MessageBox.Show("test");

            stream.Read(buffer, 0, buffer.Length);
            var username = Encoding.UTF8.GetString(buffer);
            string message = username + " has joined the session";
            //MessageBox.Show($"{username} has joined the session");
            var test = "hey";
            string mes = "hey" + "123";
        }
        public void StopServer() 
        {
            server.Stop();

            source.Cancel();
            source.Dispose();
        }

    }
}
