using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class ShowSessionParameterWindowEventArgs : EventArgs
    {
        public bool IsShown { get; set; }
        public ServerResponse ServerResponse { get; set; }
    }

    public delegate void ShowSessionParameterWindowEventHandler(Object sender, ShowSessionParameterWindowEventArgs e);
}
