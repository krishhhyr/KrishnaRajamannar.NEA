using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System.Windows;

namespace KrishnaRajamannar.NEA.Services
{
    public class ClientService : IClientService
    {
        public ClientService()
        {
            
        }

        public void ConnectToServer(string username) 
        {
            try 
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    HandleClientRequests(username);
                });
            } 
            catch  
            {
                
            }
        }
        public void HandleClientRequests(string username) 
        {
            // Temporary lines of code

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.65"), 60631);

            using var client = new TcpClient();
            client.Connect(endPoint);

            using NetworkStream stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(username);

            stream.Write(messageBytes, 0, messageBytes.Length);

            var buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            var messageFromServer = Encoding.UTF8.GetString(buffer);
            //MessageBox.Show(messageFromServer);

            stream.Flush();
        }
    }
}
