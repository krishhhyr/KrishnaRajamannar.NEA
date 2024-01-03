using KrishnaRajamannar.NEA.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class EndQuizEventArgs
    {
        ServerResponse ServerResponse { get; set; }
    }
    public delegate void EndQuizEventHandler(Object sender, EndQuizEventArgs e);
}
