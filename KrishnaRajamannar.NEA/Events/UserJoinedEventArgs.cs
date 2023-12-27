using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class UserJoinedEventArgs : EventArgs
    {
        public string? SessionId { get; set; }
        public string Username { get; set; }
    }
    public delegate void UserJoinedEventHandler(Object sender, UserJoinedEventArgs e);
}
