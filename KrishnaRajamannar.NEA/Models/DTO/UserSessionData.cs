using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    [Serializable]
    public class UserSessionData
    {
        public string SessionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}
