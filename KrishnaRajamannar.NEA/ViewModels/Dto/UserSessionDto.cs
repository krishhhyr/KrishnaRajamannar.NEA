using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels.Dto
{
    [Serializable]
    public class UserSessionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SessionId { get; set; }
    }
}
