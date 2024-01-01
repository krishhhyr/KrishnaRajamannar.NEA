using KrishnaRajamannar.NEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class QuestionRecievedEventArgs
    {
        public QuestionModel QuestionData { get; set; }
    }
    public delegate void QuestionRecievedEventHandler(Object sender, QuestionRecievedEventArgs e);
}
