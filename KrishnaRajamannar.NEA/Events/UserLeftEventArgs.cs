using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used for when a user disconnects from a session
    public class UserLeftEventArgs : EventArgs
    {
        public string Username { get; set; }
    }
    public delegate void UserLeftEventHandler(Object sender, UserLeftEventArgs e);
}
