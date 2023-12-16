namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IClientService
    {
        public void ConnectToServer(string username);

        public void HandleClientRequests(string username);
    }
}