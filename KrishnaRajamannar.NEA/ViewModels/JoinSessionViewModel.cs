using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Services;
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

        private readonly ISessionService _sessionService;

        public int UserID;
        public string Username;

        public JoinSessionViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
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

        public void IsSessionIDInputExist()
        {
            if (_sessionService.IsSessionIDExist(SessionIDInput) != true)
            {
                ShowMessageDialog("Session ID not found.");
            }
            else 
            {
                ShowMessageDialog("Session ID found.");
                ShowMessageDialog("Connecting...");
            }
        }
    }
}
