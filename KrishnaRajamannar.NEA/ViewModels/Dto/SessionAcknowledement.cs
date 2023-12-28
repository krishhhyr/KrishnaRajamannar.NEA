using System;

namespace KrishnaRajamannar.NEA.ViewModels.Dto
{
    [Serializable]
    public class SessionAcknowledement
    {
        public string SessionId { get; set; }

        public string QuizSelected { get; set; }
        
        UserSessionDto[] UserSessions { get; set; }
    }
}
