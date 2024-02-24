using System;

namespace KrishnaRajamannar.NEA.Events
{
    // Used to display a new window 
    public class ShowWindowEventArgs : EventArgs
    {
        // Used to show the window
        public bool IsShown { get; set; }
    }
    public delegate void ShowWindowEventHandler(Object sender, ShowWindowEventArgs e);
    
}
