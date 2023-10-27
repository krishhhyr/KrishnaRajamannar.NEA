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
    //public class HideWindowEventArgs : EventArgs
    //{
    //  public bool IsHidden { get; set; }
    //}

    //public delegate void HideWindowEventHandler(Object sender, HideWindowEventArgs e);

    //public class ShowWindowEventArgs : EventArgs
    //{
    //    public bool IsShow { get; set; }
    //}

    //public delegate void ShowWindowEventHandler(Object sender, ShowWindowEventArgs e);

    public class MainMenuViewModel
    {
        private readonly AccountLogin _accountLogin;

        private readonly MainMenu _mainMenu;

        public event HideWindowEventHandler HideMainMenuWindow;
        public event ShowWindowEventHandler ShowAccountLoginWindow;

        private ViewQuizzes _viewQuizzes;
        private readonly ViewLeaderboard _viewLeaderboard;
        private readonly HostSession _hostSession;
        private readonly JoinSession _joinSession;

        private  UserModel _userModel;
        private UserViewModel _userViewModel;

        private string Username; 

        public MainMenuViewModel()
        {
            //_mainMenu = new MainMenu(_userViewModel, this);

            //_accountLogin = new AccountLogin(_userViewModel);
            _viewLeaderboard = new ViewLeaderboard();
            _hostSession = new HostSession();
            _joinSession = new JoinSession();
                       

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
            //_mainMenu.Visibility = Visibility.Hidden;
            //_accountLogin.Visibility = Visibility.Visible;

            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideMainMenuWindows(args);

        }
        public void ViewQuizzes() 
        {
            if (App.ServiceProvider == null)
            {
                return;
            }
            
            _userModel = App.ServiceProvider.GetService(typeof(UserModel)) as UserModel;

            _viewQuizzes = new ViewQuizzes(_userModel.UserID);

            //_viewQuizzes.Show();

            _mainMenu.Visibility = Visibility.Hidden;
            _viewQuizzes.Visibility = Visibility.Visible;
        }
        public void ShowLeaderboard() 
        {
            _mainMenu.Visibility = Visibility.Hidden;
            _viewLeaderboard.Visibility = Visibility.Visible;
        }

        public void ShowHostSession() 
        {
            _mainMenu.Visibility = Visibility.Hidden;
            _hostSession.Visibility = Visibility.Visible;
        }

        public void ShowJoinSession()
        {
            _mainMenu.Visibility = Visibility.Hidden;
            _joinSession.Visibility= Visibility.Visible;
        }

        protected virtual void OnHideMainMenuWindows(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideMainMenuWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
