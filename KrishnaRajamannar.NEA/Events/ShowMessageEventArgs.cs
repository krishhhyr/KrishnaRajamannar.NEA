using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class ShowMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public delegate void ShowMessageEventHandler(Object sender, ShowMessageEventArgs e);
}
