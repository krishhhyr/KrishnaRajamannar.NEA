using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    public class UserConnectionService
    {
        Queue<string> joiningUsers = new Queue<string>();

        Queue<string> leavingUsers = new Queue<string>();

        ObservableCollection<string> users = new ObservableCollection<string>();

        public UserConnectionService()
        {
            
        }

        public void JoiningSession(string username) 
        {
            joiningUsers.Enqueue(username);
            users.Add(username);
        }
        public void LeavingSession(string username) 
        {
            leavingUsers.Enqueue(username);
            users.Remove(username);
        }
    }
}
