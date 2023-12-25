using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class HideWindowEventArgs : EventArgs
    {
        public bool IsHidden { get; set; }
    }

    public delegate void HideWindowEventHandler(Object sender, HideWindowEventArgs e);


    public class ClientConnectedEventArgs:EventArgs
    {
        public string SessionId { get; set; }
        public string SessionName { get; set; }
    }

    public delegate void ClientConnectedEventHandler(Object sender, ClientConnectedEventArgs e);
}
