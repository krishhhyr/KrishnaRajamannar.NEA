using KrishnaRajamannar.NEA.Models.DTO;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to process the client responses that a server recieves 
    public class ProcessClientResponseEventArgs
    {
        public ClientResponse ClientResponse { get; set; }
    }
    public delegate void ProcessClientResponseEventHandler(Object sender, ProcessClientResponseEventArgs e);
}
