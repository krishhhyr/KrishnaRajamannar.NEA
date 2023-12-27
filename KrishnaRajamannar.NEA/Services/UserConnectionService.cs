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

        public void JoiningSession(string username) 
        {
            joiningUsers.Enqueue(username);

            //var args = new UserJoinedEventArgs() { UserName = username };
            //OnUserJoinSession(args);

            //new code - k
            RetrieveUsernameJoined(username);
        }
        public void LeavingSession(string username) 
        {
            leavingUsers.Enqueue(username);
            OnLeft(new UserLeftEventArgs() { UserName = username });
        }

        //new code - k
        private void RetrieveUsernameJoined(string username) 
        {
            UserJoinedEventArgs args = new UserJoinedEventArgs();
            args.Username = username;

            OnUserJoinSession(args);
        }

        protected virtual void OnUserJoinSession(UserJoinedEventArgs e)
        {
            UserJoinedEventHandler handler = UserJoined;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLeft(UserLeftEventArgs e)
        {
            UserLeftEventHandler handler = UserLeft;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
