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
using KrishnaRajamannar.NEA.ViewModels.Dto;

namespace KrishnaRajamannar.NEA.Services
{
    public class UserConnectionService  
    {

        public event UserJoinedEventHandler UserJoined;
        public event UserLeftEventHandler UserLeft;

        public void UserJoinedSession(UserSessionDto dto) 
        {           
            ShowUsernameJoinedSession(dto);
        }

        private void ShowUsernameJoinedSession(UserSessionDto dto) 
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
