using KrishnaRajamannar.NEA.ViewModels.Dto;
using System.Collections;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IUserSessionService
    {
        void InsertUserSessionDetails(UserSessionData dto);
        void RemoveUserSessionDetails(UserSessionData dto);
        IList<UserSessionData> GetUserSessionDetails(UserSessionData dto);
    }
}