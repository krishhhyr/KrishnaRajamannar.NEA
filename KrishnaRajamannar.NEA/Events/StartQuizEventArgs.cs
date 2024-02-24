using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to process a server response that the client recieves which relates
    // to starting a quiz to review with multiple users 
    public class StartQuizEventArgs : EventArgs
    {
        public ServerResponse ServerResponse { get; set; }
    }
    public delegate void StartQuizEventHandler(Object sender, StartQuizEventArgs e);
}

