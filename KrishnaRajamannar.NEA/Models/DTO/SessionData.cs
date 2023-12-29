using System;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    [Serializable]
    public class SessionData
    {
        public string QuizSelected { get; set; }

        public IList<UserSessionData> UserSessions { get; set; }
    }
}
