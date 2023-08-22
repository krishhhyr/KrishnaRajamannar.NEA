using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    // A class which defines the properties that a question can have.
    public class QuestionModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        // Options 1 to 6 have string? which indicates that these variable accept null values.
        // Options itself represent the different answers a user can provide for a multiple choice question.

        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? Option6 { get; set; }

        // This variable represents how many seconds a user has to answer a question.
        public int Duration { get; set; }
        public int NumberOfPoints { get; set; }
    }
}
