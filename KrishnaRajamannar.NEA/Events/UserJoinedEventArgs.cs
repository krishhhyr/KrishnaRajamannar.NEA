using KrishnaRajamannar.NEA.ViewModels.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class UserJoinedEventArgs : EventArgs
    {
       public UserSessionDto UserSession { get; set; }
    }
    public delegate void UserJoinedEventHandler(Object sender, UserJoinedEventArgs e);
}
