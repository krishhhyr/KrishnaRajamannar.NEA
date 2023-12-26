using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Interfaces;
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
        public event HideWindowEventHandler HideJoinSessionWindow;
        private readonly IClientService _clientService;
        private readonly ISessionService _sessionService;

        public int UserID;
        public string Username;

        public JoinSessionViewModel(ISessionService sessionService, IClientService clientService)
        {
            _sessionService = sessionService;
            _clientService = clientService;
            _clientService.ClientConnected += _clientService_ClientConnected;
        }

        private void _clientService_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ;
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

                ConnectionStatus = _clientService.ConnectToServer(Username, ipAddressConnect, portNumberConnect, SessionIDInput.ToString());
                return true;
            }
        }
    }
}
