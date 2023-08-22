using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    // This class defines the properties of a user only.
    public class User
    {
        public User()
        {

        }

        public string username;
        public string password;
        public int points;
    }

    public class QuizTitle
    {
        public string Title { get; set; }
        public int NumberOfQuestions { get; set; }
    }
}
