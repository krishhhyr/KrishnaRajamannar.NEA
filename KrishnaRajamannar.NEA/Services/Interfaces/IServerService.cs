using System.Net.Sockets;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IServerService
    {
        public void StartServer(string hostname, string ipAddress, int portNumber);

        public void ListeningForConnections(string ipAddress, int portNumber);

        public void HandleClientRequests(TcpListener server);

        public void SendMessageToClients(string message);

        public void StopServer();
    }
}