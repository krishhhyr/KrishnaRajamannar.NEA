namespace KrishnaRajamannar.NEA.Services
{
    public interface ISessionService
    {
        public bool IsSessionIDExist(int sessionIDInput);

        public bool IsPortNumberExist(int portNumberInput);

        public bool InsertSessionData(int sessionID, string IPAddress, int portNumber, int quizID);

        public (string, int) GetConnectionData(int sessionID);
    }
}