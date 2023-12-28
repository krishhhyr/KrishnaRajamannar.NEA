using KrishnaRajamannar.NEA.Events;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IClientService
    {
        public event ClientConnectedEventHandler ClientConnected;
        public string ConnectToServer(string username, int userId, string ipAddressConnect, int portNumberConnect, string sessionId);

        public string HandleClientRequests(string username, int userID, string ipAddressConnect, int portNumberConnect, string sessionId);
    }
}