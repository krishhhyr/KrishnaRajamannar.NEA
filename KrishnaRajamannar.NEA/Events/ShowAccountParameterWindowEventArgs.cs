using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class ShowAccountParameterWindowEventArgs : EventArgs
    {
        public bool IsShown { get; set; }
        public int? UserID { get; set; }
        public string? Username { get; set; }
        public int? TotalPoints { get; set; }

    }
    public delegate void ShowAccountParameterWindowEventHandler(Object sender, ShowAccountParameterWindowEventArgs e);
}
