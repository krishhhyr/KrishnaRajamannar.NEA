using System;

namespace KrishnaRajamannar.NEA.Events
{
    // Used to display a new window which passes information about the user who has logged
    // into the application
    public class ShowAccountParameterWindowEventArgs : EventArgs
    {
        public bool IsShown { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int TotalPoints { get; set; }

    }
    // This is the method that will handle the event 
    public delegate void ShowAccountParameterWindowEventHandler(Object sender, ShowAccountParameterWindowEventArgs e);
}
