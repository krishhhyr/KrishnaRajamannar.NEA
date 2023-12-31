using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.Views;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
        public event ShowAccountParameterWindowEventHandler ShowHostSessionWindow;
        public event ShowAccountParameterWindowEventHandler ShowJoinSessionWindow;

        public event ShowSessionParameterWindowEventHandler ShowClientSessionWindow;

        public event ShowWindowEventHandler ShowAccountLoginWindow;
        public event HideWindowEventHandler HideMainMenuWindow;

        public AccountLoginViewModel AccountLoginViewModel;
        public ViewQuizzesViewModel ViewQuizzesViewModel;
        public ViewLeaderboardViewModel ViewLeaderboardViewModel;
        public HostSessionViewModel HostSessionViewModel;
        public JoinSessionViewModel JoinSessionViewModel;
        public ServerSessionViewModel ServerSessionViewModel;
        public ClientSessionViewModel ClientSessionViewModel;

        private readonly ISessionService _sessionService;
        private readonly IClientService _clientService;

        public MainMenuViewModel(ISessionService sessionService, IClientService clientService)
        {
            AccountLoginViewModel = App.ServiceProvider.GetService(typeof(AccountLoginViewModel)) as AccountLoginViewModel;
            ViewQuizzesViewModel = App.ServiceProvider.GetService(typeof(ViewQuizzesViewModel)) as ViewQuizzesViewModel;
            ViewLeaderboardViewModel = App.ServiceProvider.GetService(typeof(ViewLeaderboardViewModel)) as ViewLeaderboardViewModel;
            HostSessionViewModel = App.ServiceProvider.GetService(typeof(HostSessionViewModel)) as HostSessionViewModel;
            JoinSessionViewModel = App.ServiceProvider.GetService(typeof(JoinSessionViewModel)) as JoinSessionViewModel;

            ServerSessionViewModel = App.ServiceProvider.GetService(typeof(ServerSessionViewModel)) as ServerSessionViewModel;
            ClientSessionViewModel = App.ServiceProvider.GetService(typeof(ClientSessionViewModel)) as ClientSessionViewModel;

            _sessionService = sessionService;
            _clientService = clientService;

            _clientService.ClientConnected += OnClientConnected;

        }

        #region Properties

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
        private string _connectionMessage;
        public string ConnectionMessage
        {
            get { return _connectionMessage; }
            set
            {
                _connectionMessage = value;
                RaisePropertyChange("ConnectionMessage");
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

        // Event for when a client is first connected to the server
        // This passes the data sent from the client to a function which displays the client window
        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ShowClientSession(e.ServerResponse);
        }

        private void ShowClientSession(ServerResponse response)
        {
            ShowSessionParameterWindowEventArgs args = new ShowSessionParameterWindowEventArgs();
            args.IsShown = true;
            args.ServerResponse = response;
            OnShowClientSessionWindow(args);
        }

        protected virtual void OnShowClientSessionWindow(ShowSessionParameterWindowEventArgs e)
        {
            ShowSessionParameterWindowEventHandler handler = ShowClientSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

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
        public void ShowHostSession()
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = _userid;
            args.Username = _username;
            OnShowHostSessionWindow(args);

        }
        protected virtual void OnShowHostSessionWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowHostSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void ShowJoinSession()
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = _userid;
            args.Username = _username;
            OnShowJoinSessionWindow(args);

        }
        
        protected virtual void OnShowJoinSessionWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowJoinSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
       
        #endregion

        public bool JoinSession() 
        {
            if (_sessionService.IsSessionIDExist(SessionID) != true)
            {
                ConnectionMessage = "Session ID not found";
                return false;
            }
            else
            {
                (string, int) connectionInfo = _sessionService.GetConnectionData(SessionID);

                string ipAddressConnect = connectionInfo.Item1;
                int portNumberConnect = connectionInfo.Item2;

                _clientService.ConnectToServer(Username, UserID, ipAddressConnect, portNumberConnect, SessionID.ToString());
                ConnectionMessage = "Connecting...";
                return true;
            }
        }
    }
}
