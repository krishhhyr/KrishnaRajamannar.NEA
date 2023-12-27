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

        public string ConnectToServer(string username, string ipAddressConnect, int portNumberConnect, string sessionId) 
        {
            string messageFromServer = "";

            try 
            {
                Task task = Task.Factory.StartNew(() =>
                {
                   messageFromServer = HandleClientRequests(username, ipAddressConnect, portNumberConnect, sessionId);
                });

            } 
            catch  
            {
                
            }

            return messageFromServer;
        }
        public string HandleClientRequests(string username, string ipAddressConnect, int portNumberConnect, string sessionId) 
        {
            var buffer = new byte[1024];

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddressConnect), portNumberConnect);

            using var client = new TcpClient();
            client.Connect(endPoint);

            using NetworkStream stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(username);

            stream.Write(messageBytes, 0, messageBytes.Length);

            string messageFromServer = "";

            if (stream.DataAvailable == true) 
            {
                var reading = stream.Read(buffer, 0, buffer.Length);
                messageFromServer = Encoding.UTF8.GetString(buffer, 0, reading);
            }

            stream.Flush();
           

            //Raise the Event 
            OnClientConnected(new ClientConnectedEventArgs() { SessionId =sessionId });

            return messageFromServer;
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
