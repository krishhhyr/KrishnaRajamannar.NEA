using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services.Interfaces;

namespace KrishnaRajamannar.NEA.Services.Connection
{
    // This class represents the users who have joined and left a session.

    public class UserConnectionService
    {

        public event UserJoinedEventHandler UserJoined;
        public event UserLeftEventHandler UserLeft;
        public IUserSessionService UserSessionService;

        public UserConnectionService(IUserSessionService userSessionService)
        {
            UserSessionService = userSessionService;
        }

        // This is used to insert user data into the database for when a user joins a session
        public void UserJoinedSession(UserSessionData dto)
        {
            UserSessionService.InsertUserSessionDetails(dto);
            ShowUsernameJoinedSession(dto);
        }

        // This is an event which is used to show the username of users who have joined a session
        private void ShowUsernameJoinedSession(UserSessionData dto)
        {
            UserJoinedEventArgs args = new UserJoinedEventArgs();
            args.UserSession = dto;
            OnShowUsernameJoinedSession(args);

        }

        protected virtual void OnShowUsernameJoinedSession(UserJoinedEventArgs e)
        {
            UserJoinedEventHandler handler = UserJoined;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // This is used to show the names of users who have left a session.
        public void UserLeftSession(string username)
        {
            ShowUsernameLeftSession(username);
        }

        private void ShowUsernameLeftSession(string username)
        {
            UserLeftEventArgs args = new UserLeftEventArgs();
            args.Username = username;

            OnShowUsernameLeftSession(args);
        }

        protected virtual void OnShowUsernameLeftSession(UserLeftEventArgs e)
        {
            UserLeftEventHandler handler = UserLeft;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
