using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class UserLeftEventArgs : EventArgs
    {
        public string? SessionId { get; set; }
        public string? UserName { get; set; }
    }
    public delegate void UserLeftEventHandler(Object sender, UserLeftEventArgs e);
}
