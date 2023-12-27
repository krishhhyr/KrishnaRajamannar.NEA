using KrishnaRajamannar.NEA.Events;
using System.Collections.ObjectModel;

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