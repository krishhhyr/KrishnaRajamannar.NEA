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

        public event ShowAccountParameterWindowEventHandler ShowViewQuizzesWindow;

        public event ShowWindowEventHandler ShowLeaderboardWindow;

        public event ShowWindowEventHandler ShowHostSessionWindow;

        public event ShowWindowEventHandler ShowJoinSessionWindow;

        public event ShowWindowEventHandler ShowAccountLoginWindow;

        public event HideWindowEventHandler HideMainMenuWindow;

        // need to add other view models - leaderboardVM, hostsessionVM and joinsessionVM

        public AccountLoginViewModel AccountLoginViewModel;

        public ViewQuizzesViewModel ViewQuizzesViewModel;

        public MainMenuViewModel()
        {
            AccountLoginViewModel = App.ServiceProvider.GetService(typeof(AccountLoginViewModel)) as AccountLoginViewModel;
            ViewQuizzesViewModel = App.ServiceProvider.GetService(typeof(ViewQuizzesViewModel)) as ViewQuizzesViewModel;
        }
        private int _userid;
        public int UserID
        {
            get { return _userid; }
            set
            {
                _userid = value;
                RaisePropertyChange("UserID");
            }
        }
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChange("Username");
            }
        }
        private int _totalpoints;
        public int TotalPoints
        {
            get { return _totalpoints; }
            set
            {
                _totalpoints = value;
                RaisePropertyChange("TotalPoints");
            }
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
        private void ShowAccountLogin()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowAccountLoginWindow(args);

        }
        private void ShowViewQuizzes()
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = _userid;
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
        protected virtual void OnShowAccountLoginWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowAccountLoginWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowViewQuizzesWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowViewQuizzesWindow;
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
        public void DisplayAccountLoginWindow() 
        {
            ShowAccountLogin();
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
    }
}
