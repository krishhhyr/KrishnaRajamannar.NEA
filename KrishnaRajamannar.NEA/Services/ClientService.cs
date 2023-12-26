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

namespace KrishnaRajamannar.NEA.Services
{
    public class ClientService : IClientService
    {
        //Create a event handler 
        public event ClientConnectedEventHandler ClientConnected;
        private readonly string sessionId;

        public ClientService()
        {
            
        }

        public void ConnectToServer(string username, string sessionId) 
        {
            try 
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    HandleClientRequests(username,sessionId);
                });
            } 
            catch  
            {
                
            }
        }
        public void HandleClientRequests(string username, string sessionId) 
        {
            //


            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.65"), 60631);

            using var client = new TcpClient();
            client.Connect(endPoint);

            using NetworkStream stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(username);

            stream.Write(messageBytes, 0, messageBytes.Length);

            var buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            var messageFromServer = Encoding.UTF8.GetString(buffer);
            stream.Flush();
           

            //Raise the Event 
            OnClientConnected(new ClientConnectedEventArgs() { SessionId =sessionId });
        }

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            ClientConnectedEventHandler handler = ClientConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
