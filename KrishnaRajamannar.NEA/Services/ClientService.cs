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

        public void ConnectToServer() 
        {
            try 
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    HandleClientRequests();
                });
            } 
            catch  
            {
                
            }
        }
        public void HandleClientRequests() 
        {
            // Temporary lines of code...

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(iPAddress, 13);

            using var client = new TcpClient();
            client.Connect(endPoint);

            using NetworkStream stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes("hey");
            stream.Write(messageBytes, 0, messageBytes.Length);

            var buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            var messageFromServer = Encoding.UTF8.GetString(buffer);
            MessageBox.Show(messageFromServer);

            stream.Flush();
        }
    }
}
