using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    // This class defines the properties of a user only.
    public class UserModel
    {
      
        public string Username { get; set; }
        public int UserID { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
    }
}
