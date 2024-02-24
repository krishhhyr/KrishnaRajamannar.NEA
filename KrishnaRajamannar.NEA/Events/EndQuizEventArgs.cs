using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to process a server response that the client receives which relates
    // to starting a quiz to review with multiple users 
    public class EndQuizEventArgs
    {
       public ServerResponse ServerResponse { get; set; }
    }
    public delegate void EndQuizEventHandler(Object sender, EndQuizEventArgs e);
}
