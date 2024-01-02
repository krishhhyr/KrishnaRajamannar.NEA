using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class ProcessServerResponseEventArgs : EventArgs
    {
        public ServerResponse ServerResponse { get; set; }
    }
    public delegate void ProcessServerResponseEventHandler(Object sender, ProcessServerResponseEventArgs e);
}

