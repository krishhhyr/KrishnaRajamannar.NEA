namespace KrishnaRajamannar.NEA.Services
{
    public interface IUserService
    {
        void CreateUser(string username, string password);
        int GetNumberOfRows();
        string GetPassword(string username);
        int GetPoints(string username);
        int GetUserID(string username);
        string HashPassword(string password);
        bool IsUserExists(string username);
    }
}