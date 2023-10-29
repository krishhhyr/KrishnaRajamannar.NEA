using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event ShowWindowEventHandler ShowViewQuizzesWindow;

        public event ShowWindowEventHandler ShowLeaderboardWindow;

        public event ShowWindowEventHandler ShowHostSessionWindow;

        public event ShowWindowEventHandler ShowJoinSessionWindow;

        public event HideWindowEventHandler HideMainMenuWindow;

        public int? UserID;
        public string? Username;
        public int? TotalPoints;

        public MainMenuViewModel()
        {
            
        }
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        private void HideMainMenu()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideMainMenuWindow(args);
        }
        private void ShowViewQuizzes()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowViewQuizzesWindow(args);

        }
        private void ShowLeaderboard()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowLeaderboardWindow(args);

        }
        private void ShowHostSession()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowHostSessionWindow(args);

        }
        private void ShowJoinSession()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowJoinSessionWindow(args);

        }
        protected virtual void OnShowViewQuizzesWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowViewQuizzesWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowLeaderboardWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowLeaderboardWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowHostSessionWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowHostSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowJoinSessionWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowJoinSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnHideMainMenuWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideMainMenuWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void DisplayViewQuizzesWindow() 
        {
            ShowViewQuizzes();
        }
        public void DisplayLeaderboardWindow() 
        {
            ShowLeaderboard();
        }
        public void DisplayHostSessionWindow() 
        {
            ShowHostSession();
        }
        public void DisplayJoinSessionWindow() 
        {
            ShowJoinSession();
        }
        public void CloseMainMenuWindow() 
        {
            HideMainMenu();
        }

        private readonly AccountLogin _accountLogin;

        private readonly MainMenu _mainMenu;

        //public event HideWindowEventHandler HideMainMenuWindow;
        //public event ShowWindowEventHandler ShowAccountLoginWindow;

        private ViewQuizzes _viewQuizzes;
        private readonly ViewLeaderboard _viewLeaderboard;
        private readonly HostSession _hostSession;
        private readonly JoinSession _joinSession;

        private  UserModel _userModel;
        private UserViewModel _userViewModel;

        //private string Username;


        //public MainMenuViewModel()
        //{
        //    //_mainMenu = new MainMenu(_userViewModel, this);

        //    //_accountLogin = new AccountLogin(_userViewModel);
        //    _viewLeaderboard = new ViewLeaderboard();
        //    _hostSession = new HostSession();
        //    _joinSession = new JoinSession();
                       

        //}

        //public void Logout() 
        //{
        //    if (MessageBox.Show("Do you want to log out of this account?", "Account Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        ShowAccountLogin();
        //    }
        //}

        //public void ShowAccountLogin() 
        //{
        //    //_mainMenu.Visibility = Visibility.Hidden;
        //    //_accountLogin.Visibility = Visibility.Visible;

        //    //HideWindowEventArgs args = new HideWindowEventArgs();
        //    //args.IsHidden = true;
        //    //OnHideMainMenuWindows(args);

        //}
        //public void ViewQuizzes() 
        //{
        //    if (App.ServiceProvider == null)
        //    {
        //        return;
        //    }
            
        //    _userModel = App.ServiceProvider.GetService(typeof(UserModel)) as UserModel;

        //    _viewQuizzes = new ViewQuizzes(_userModel.UserID);

        //    //_viewQuizzes.Show();

        //    _mainMenu.Visibility = Visibility.Hidden;
        //    _viewQuizzes.Visibility = Visibility.Visible;
        //}
        //public void ShowLeaderboard() 
        //{
        //    _mainMenu.Visibility = Visibility.Hidden;
        //    _viewLeaderboard.Visibility = Visibility.Visible;
        //}

        //public void ShowHostSession() 
        //{
        //    _mainMenu.Visibility = Visibility.Hidden;
        //    _hostSession.Visibility = Visibility.Visible;
        //}

        //public void ShowJoinSession()
        //{
        //    _mainMenu.Visibility = Visibility.Hidden;
        //    _joinSession.Visibility= Visibility.Visible;
        //}

        //protected virtual void OnHideMainMenuWindows(HideWindowEventArgs e)
        //{
        //    HideWindowEventHandler handler = HideMainMenuWindow;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}
    }
}
