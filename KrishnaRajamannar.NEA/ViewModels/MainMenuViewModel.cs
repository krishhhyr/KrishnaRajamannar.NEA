using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class MainMenuViewModel
    {
        private readonly AccountLogin _accountLogin;
        private ViewQuizzes _viewQuizzes;
        private readonly ViewLeaderboard _viewLeaderboard;
        private readonly HostSession _hostSession;
        private readonly JoinSession _joinSession;
        private  UserModel _userModel;

        readonly string Username; 

        public MainMenuViewModel()
        {
                       

        }

        public void Logout() 
        {
            if (MessageBox.Show("Do you want to log out of this account?", "Account Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AccountLoginWindow();
            }
        }

        public void AccountLoginWindow() 
        {
            if (_accountLogin != null) { _accountLogin.Show();}
        }
        public void ViewQuizzes(int userID) 
        {
            if (App.ServiceProvider == null) return;           
            _userModel = App.ServiceProvider.GetService(typeof(UserModel)) as UserModel;
            _viewQuizzes = new ViewQuizzes(_userModel.UserID);
            _viewQuizzes.Show(); 
        }
        public void LeaderboardWindow() 
        {
            if (_viewLeaderboard != null) { _viewLeaderboard.Show();}
        }

        public void HostSessionWindow() 
        {
            if (_hostSession != null) { _hostSession.Show();}
        }

        public void JoinSessionWindow()
        {
            if (_joinSession != null) { _joinSession.Show();}
        }
    }
}
