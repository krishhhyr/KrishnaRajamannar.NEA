using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class ShowQuizParameterWindowEventArgs
    {
        public bool IsShown { get; set; }
        public int? QuizID { get; set; }
    }
    public delegate void ShowQuizParameterWindowEventHandler(Object sender, ShowQuizParameterWindowEventArgs e);
}
