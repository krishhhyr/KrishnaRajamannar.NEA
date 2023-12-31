﻿using KrishnaRajamannar.NEA.Models.Dto;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    public class StartQuizEventArgs : EventArgs
    {
        public ServerResponse ServerResponse { get; set; }
    }
    public delegate void StartQuizEventHandler(Object sender, StartQuizEventArgs e);
}

