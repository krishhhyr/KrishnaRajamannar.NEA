using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Services.Interfaces;

namespace KrishnaRajamannar.NEA.Services
{
    public class UserConnectionService //: IUserConnectionService
    {
        private Queue<string> joiningUsers = new Queue<string>();

        private Queue<string> leavingUsers = new Queue<string>();

        public event UserJoinedEventHandler UserJoined;
        public event UserLeftEventHandler UserLeft;

        public void UserJoinedSession(string username) 
        {
            joiningUsers.Enqueue(username);
            ShowUsernameJoinedSession(username);
        }

        private void ShowUsernameJoinedSession(string username) 
        {
            UserJoinedEventArgs args = new UserJoinedEventArgs();
            args.Username = username;

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

        public void UserLeftSession(string username)
        {
            joiningUsers.Dequeue();
            leavingUsers.Enqueue(username);

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
