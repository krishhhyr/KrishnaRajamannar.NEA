using System;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Models.Dto

{   //This means that this class can be converted into a byte stream
    // It is used to pass the session data to a user who has joined the session
    [Serializable]
    // This is a class which represents the general data for a created session
    // Outlines how the review of a quiz will end
    public class SessionData
    {
        public string HostName { get; set; }
        public string QuizSelected { get; set; }
        public string EndQuizCondition { get; set; }
        public string EndQuizConditionValue { get; set; }
        // This is an IList of UserSessionData which represents all the users who have joined the session
        public IList<UserSessionData> UserSessions { get; set; }
    }
}
