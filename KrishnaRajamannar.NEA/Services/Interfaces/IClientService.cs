using KrishnaRajamannar.NEA.Events;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IClientService
    {
        public event ClientConnectedEventHandler ClientConnected;
        public void ConnectToServer(string username, string sessionId);

        public void HandleClientRequests(string username, string sessionId);
    }
}