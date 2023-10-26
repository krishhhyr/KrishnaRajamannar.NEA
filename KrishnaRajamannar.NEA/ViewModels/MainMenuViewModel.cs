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

        private readonly MainMenu _mainMenu;

        private ViewQuizzes _viewQuizzes;
        private readonly ViewLeaderboard _viewLeaderboard;
        private readonly HostSession _hostSession;
        private readonly JoinSession _joinSession;

        private  UserModel _userModel;
        private UserViewModel _userViewModel;

        private string Username; 

        public MainMenuViewModel()
        {
            _accountLogin = new AccountLogin(_userViewModel);

            _mainMenu = new MainMenu(_userViewModel, this);
                       

        }

        public void Logout() 
        {
            if (MessageBox.Show("Do you want to log out of this account?", "Account Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ShowAccountLogin();
            }
        }

        public void ShowAccountLogin() 
        {
            _mainMenu.Visibility = Visibility.Hidden;

            _accountLogin.Visibility = Visibility.Visible;
        }
        public void ViewQuizzes(int userID) 
        {
            if (App.ServiceProvider == null) return;           
            _userModel = App.ServiceProvider.GetService(typeof(UserModel)) as UserModel;
            _viewQuizzes = new ViewQuizzes(_userModel.UserID);
            _viewQuizzes.Show(); 
        }
        public void ShowLeaderboard() 
        {
            if (_viewLeaderboard != null) 
            { 
                _viewLeaderboard.Show();
            }
        }

        public void ShowHostSession() 
        {
            if (_hostSession != null) 
            { 
                _hostSession.Show();
            }
        }

        public void ShowJoinSession()
        {
            if (_joinSession != null) 
            { 
                _joinSession.Show();
            }
        }
    }
}
