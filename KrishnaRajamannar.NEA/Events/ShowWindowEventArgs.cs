using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class ShowWindowEventArgs : EventArgs
    {
        public bool IsShown { get; set; }
    }
    public delegate void ShowWindowEventHandler(Object sender, ShowWindowEventArgs e);
}
