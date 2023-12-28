using KrishnaRajamannar.NEA.ViewModels.Dto;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IUserSessionService
    {
        void GetUserSessionDetails(UserSessionDto dto);
        void InsertUserSessionDetails(UserSessionDto dto);
        void RemoveUserSessionDetails(UserSessionDto dto);


    }
}