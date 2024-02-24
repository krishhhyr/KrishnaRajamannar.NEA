using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to process the server responses that a client recieves 
    public class ProcessServerResponseEventArgs : EventArgs
    {
        public ServerResponse ServerResponse { get; set; }
    }
    public delegate void ProcessServerResponseEventHandler(Object sender, ProcessServerResponseEventArgs e);
}

