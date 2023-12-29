using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class UserJoinedEventArgs : EventArgs
    {
       public UserSessionData UserSession { get; set; }
    }
    public delegate void UserJoinedEventHandler(Object sender, UserJoinedEventArgs e);
}
