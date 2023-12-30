using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class JoinSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ShowMessageEventHandler ShowMessage;
        public event ShowSessionParameterWindowEventHandler ShowViewSessionInfoWindow;
        public event HideWindowEventHandler HideJoinSessionWindow;
        private readonly IClientService _clientService;
        private readonly ISessionService _sessionService;
        private readonly UserConnectionService _userConnectionService;

        private ViewSessionInformation viewSessionInformation;
        private ViewSessionInfoViewModel viewSessionInfoViewModel;

        public int UserID;
        public string Username;

        public JoinSessionViewModel(ISessionService sessionService, IClientService clientService, UserConnectionService userConnectionService)
        {
            _sessionService = sessionService;
            _clientService = clientService;
            _userConnectionService = userConnectionService;

            _clientService.ClientConnected += OnClientConnected;
            _clientService.
        }

        // I need to show the Viewsession info window 
        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ShowViewSessionInfo(e.ServerResponse);
        }

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private int _sessionIDInput;
        public int SessionIDInput 
        {
            get { return _sessionIDInput; }
            set 
            {
                _sessionIDInput = value;
                RaisePropertyChange("SessionID");
            }
        }
        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                RaisePropertyChange("ConnectionStatus");
            }
        }

        private void ShowMessageDialog(string message)
        {
            ShowMessageEventArgs args = new ShowMessageEventArgs();
            args.Message = message;
            OnShowMessage(args);
        }

        private void ShowViewSessionInfo(ServerResponse response) 
        {
            ShowSessionParameterWindowEventArgs args = new ShowSessionParameterWindowEventArgs();
            args.IsShown = true;
            args.ServerResponse = response;
            OnShowViewSessionInfoWindow(args);
        }

        private void HideJoinSession()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideJoinSessionWindow(args);
        }

        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShowViewSessionInfoWindow(ShowSessionParameterWindowEventArgs e) 
        {
            ShowSessionParameterWindowEventHandler handler = ShowViewSessionInfoWindow;
            if (handler != null) 
            {
                handler(this, e);
            }
        }

        protected virtual void OnHideJoinSessionWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideJoinSessionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void CloseJoinSessionWindow()
        {
            HideJoinSession();

            _userConnectionService.UserLeftSession(Username);
        }

        public bool JoinSession()
        {        

            if (_sessionService.IsSessionIDExist(SessionIDInput) != true)
            {
                ConnectionStatus = "Session ID not found";
                return false;
            }
            else
            {
                (string, int) connectionInfo = _sessionService.GetConnectionData(SessionIDInput);

                string ipAddressConnect = connectionInfo.Item1;
                int portNumberConnect = connectionInfo.Item2;

                ConnectionStatus = _clientService.ConnectToServer(Username, UserID, ipAddressConnect, portNumberConnect, SessionIDInput.ToString());
                return true;
            }
        }
    }
}
