using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ViewLeaderboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event HideWindowEventHandler HideViewLeaderboardWindow;

        private readonly ILeaderboardService _leaderboardService;

        public ViewLeaderboardViewModel(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        private void HideViewLeaderboard()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideViewQuizzesWindow(args);
        }
        protected virtual void OnHideViewQuizzesWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideViewLeaderboardWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void CloseViewLeaderboardWindow() 
        {
            HideViewLeaderboard();
        }
        public IList<LeaderboardModel> GetUserLeaderboard() 
        {
            return _leaderboardService.GetLeaderboard();
        }
    }
}
