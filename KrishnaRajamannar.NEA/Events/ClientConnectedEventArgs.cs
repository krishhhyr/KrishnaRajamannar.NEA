using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used for when a client has joined a session
    // It is used to load the data into the client window on the session details as an example
    public class ClientConnectedEventArgs:EventArgs
    {
       public ServerResponse ServerResponse { get; set; }
    }
    public delegate void ClientConnectedEventHandler(Object sender, ClientConnectedEventArgs e);
}
