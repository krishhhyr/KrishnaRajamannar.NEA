using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class ClientConnectedEventArgs:EventArgs
    {
       public ServerResponse ServerResponse { get; set; }
    }
    public delegate void ClientConnectedEventHandler(Object sender, ClientConnectedEventArgs e);
    public delegate void StartQuizEventHandler(Object sender, StartQuizEventArgs e);
}
