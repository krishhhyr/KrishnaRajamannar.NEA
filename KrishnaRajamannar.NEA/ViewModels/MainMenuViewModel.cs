using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Services;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    // Inherits the INotifyPropertyChanged interface
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        // Part of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        // Events used to display new windows and pass data through these windows
        public event ShowAccountParameterWindowEventHandler ShowViewQuizzesWindow;
        public event ShowWindowEventHandler ShowLeaderboardWindow;
        public event ShowAccountParameterWindowEventHandler ShowServerSessionWindow;
        public event ShowWindowEventHandler ShowAccountLoginWindow;
        public event HideWindowEventHandler HideMainMenuWindow;

        // ViewModels which will be used to pass data through the new windows
        public AccountLoginViewModel AccountLoginViewModel;
        public ViewQuizzesViewModel ViewQuizzesViewModel;
        public ViewLeaderboardViewModel ViewLeaderboardViewModel;
        public HostSessionViewModel HostSessionViewModel;
        public JoinSessionViewModel JoinSessionViewModel;
        public ServerSessionViewModel ServerSessionViewModel;
        public ClientSessionViewModel ClientSessionViewModel;

        // Uses the ISessionService to check if the session ID inputted exists in the DB
        private readonly ISessionService _sessionService;

        public MainMenuViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;

            AccountLoginViewModel = App.ServiceProvider.GetService(typeof(AccountLoginViewModel)) as AccountLoginViewModel;
            ViewQuizzesViewModel = App.ServiceProvider.GetService(typeof(ViewQuizzesViewModel)) as ViewQuizzesViewModel;
            ViewLeaderboardViewModel = App.ServiceProvider.GetService(typeof(ViewLeaderboardViewModel)) as ViewLeaderboardViewModel;
            ServerSessionViewModel = App.ServiceProvider.GetService(typeof(ServerSessionViewModel)) as ServerSessionViewModel;
            ClientSessionViewModel = App.ServiceProvider.GetService(typeof(ClientSessionViewModel)) as ClientSessionViewModel;
        }

        #region Properties

        // Binds with the UI to display the User ID of the user who has logged in
        // This data was passed from the Account Login window through an event
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
        // Binds with the UI to display the username of the user who has logged in
        // This data was passed from the Account Login window through an event
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
        // Binds with the UI to display the number of total points gained of the user who has logged in
        // This data was passed from the Account Login window through an event
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
        // Binds to the UI to retrieve the session ID that the user has inputted
        // when attempting to join a session
        private int _sessionID;
        public int SessionID
        {
            get { return _sessionID; }
            set
            {
                _sessionID = value;
                RaisePropertyChange("SessionID");
            }
        }
        // Binds to the UI to display error messages when inputting the session ID
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChange("Message");
            }
        }
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        #endregion

        #region Events

        // This is used to hide the main menu once a new window is displayed
        public void HideMainMenu()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideMainMenuWindow(args);
        }

        protected virtual void OnHideMainMenuWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideMainMenuWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // If a user logs out, this is used to show the Account Login windo
        public void ShowAccountLogin()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowAccountLoginWindow(args);

        }

        protected virtual void OnShowAccountLoginWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowAccountLoginWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void ShowViewQuizzes()
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = _userid;
            OnShowViewQuizzesWindow(args);

        }

        protected virtual void OnShowViewQuizzesWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowViewQuizzesWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        // ShowLeaderboard() and OnShowLeaderboardWindow() are both events used to display the leaderboard window
        // if the user presses View Leaderboard in the main menu
        public void ShowLeaderboard()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowLeaderboardWindow(args);

        }
        protected virtual void OnShowLeaderboardWindow(ShowWindowEventArgs e)
        {
            ShowWindowEventHandler handler = ShowLeaderboardWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        // This is used to pass the user data from the Main Menu Window to the ServerSession window
        public void ShowServerSession() 
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = _userid;
            args.Username = _username;
            args.TotalPoints = _totalpoints;
            OnServerSessionWindow(args);

        }
        protected virtual void OnServerSessionWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowServerSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        // This is used to validate the session ID which entered by users to join a session
        // It calls the sessionService to check whether the session ID entered actually exists in the DB
        public bool ValidateSessionID() 
        {
            if (_sessionService.IsSessionIDExist(SessionID) != true) 
            {
                Message = "Session ID not Found.";
                return false;
            }
            Message = "Connecting...";
            return true;
        }
    }
}
