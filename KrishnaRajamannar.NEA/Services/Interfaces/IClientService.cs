using KrishnaRajamannar.NEA.Events;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IClientService
    {
        public event ClientConnectedEventHandler ClientConnected;
        public string ConnectToServer(string username, string ipAddressConnect, int portNumberConnect, string sessionId);

        public string HandleClientRequests(string username, string ipAddressConnect, int portNumberConnect, string sessionId);
    }
}