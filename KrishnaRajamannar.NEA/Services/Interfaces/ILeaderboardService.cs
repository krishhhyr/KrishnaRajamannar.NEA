using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface ILeaderboardService
    {
        IList<LeaderboardModel> GetLeaderboard();
    }
}