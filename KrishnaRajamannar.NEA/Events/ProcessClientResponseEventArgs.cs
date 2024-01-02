using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Events
{
    public class ProcessClientResponseEventArgs
    {
        public ClientResponse ClientResponse { get; set; }
    }
    public delegate void ProcessClientResponseEventHandler(Object sender, ProcessClientResponseEventArgs e);
}
