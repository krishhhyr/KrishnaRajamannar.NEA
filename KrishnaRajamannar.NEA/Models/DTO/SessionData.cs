using System;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    [Serializable]
    public class SessionData
    {
        public string HostName { get; set; }
        public string QuizSelected { get; set; }
        public string EndQuizCondition { get; set; }
        public string EndQuizConditionValue { get; set; }
        public IList<UserSessionData> UserSessions { get; set; }
    }
}
