using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to hide a window 
    // It would be used when a back button is pressed on a window as an examplep
    public class HideWindowEventArgs : EventArgs
    {
        public bool IsHidden { get; set; }
    }
    public delegate void HideWindowEventHandler(Object sender, HideWindowEventArgs e);
}
