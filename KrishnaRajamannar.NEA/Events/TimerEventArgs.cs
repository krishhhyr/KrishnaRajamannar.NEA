using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class TimerEventArgs
    {
        public bool TimerFinished { get; set; }
    }
    public delegate void TimerEventHandler(Object sender, TimerEventArgs e);
}
