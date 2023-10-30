using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface IUserService
    {
        public IList<UserModel> GetUserDetails(string _username);
        bool IsUserExists(string username);
        string HashPassword(string password);
        void CreateUser(string username, string password);
        void UpdatePoints(int userID, int pointsGained);
    }
}