using System;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    // This means it can be converted into a byte stream
    // It is used when the user connecting a session passes their data to the host 
    // so that the host can see who has joined the session
    [Serializable]
    // This is a class which represents the data of a user who has joined a session
    public class UserSessionData
    {
        public string SessionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int TotalPoints { get; set; }
    }
}
