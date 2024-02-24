using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used when a user joins a session
    public class UserJoinedEventArgs : EventArgs
    {
       public UserSessionData UserSession { get; set; }
    }
    public delegate void UserJoinedEventHandler(Object sender, UserJoinedEventArgs e);
}
