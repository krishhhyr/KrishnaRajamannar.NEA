namespace KrishnaRajamannar.NEA.Services
{
    public interface ISessionService
    {
        public bool IsSessionIDExist(int sessionIDInput);

        public bool IsPortNumberExist(int portNumberInput);

        public void InsertSessionData(int sessionID, string quiz, string endQuizCondition, string endQuizValue, 
            string IPAddress, int portNumber, int quizID);

        public (string?, int?) GetConnectionData(int sessionID);
    }
}