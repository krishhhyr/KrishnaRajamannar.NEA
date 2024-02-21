using KrishnaRajamannar.NEA.Events;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IUserConnectionService
    {       
        public void JoiningSession(string username);
        public void LeavingSession(string username);

        event UserJoinedEventHandler UserJoined;
        event UserLeftEventHandler UserLeft;

    }
}