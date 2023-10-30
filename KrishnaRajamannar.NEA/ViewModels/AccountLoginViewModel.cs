using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class AccountLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event ShowMessageEventHandler ShowMessage;

        public event ShowAccountParameterWindowEventHandler ShowMainMenuWindow;

        public event ShowWindowEventHandler ShowAccountCreationWindow;

        public event HideWindowEventHandler HideAccountLoginWindow;

        private readonly IUserService _userService;

        private readonly UserModel _userModel;

        public int UserID;
        public int TotalPoints;

        public AccountLoginViewModel(IUserService userService, UserModel userModel)
        {
            _userService = userService;
            _userModel = userModel;
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
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChange("Password");
            }
        }
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        private void ShowMessageDialog(string message)
        {
            ShowMessageEventArgs args = new ShowMessageEventArgs();
            args.Message = message;
            OnShowMessage(args);
        }
        private void HideAccountLogin() 
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideAccountLoginWindow(args);
        }
        private void ShowMainMenu() 
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = UserID;
            args.Username = Username;
            args.TotalPoints = TotalPoints;
            OnShowMainMenuWindow(args);
        }
        private void ShowAccountCreation() 
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowAccountCreationWindow(args);
        }
        // Raises an event
        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnHideAccountLoginWindow(HideWindowEventArgs e) 
        {
            HideWindowEventHandler handler = HideAccountLoginWindow;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowMainMenuWindow(ShowAccountParameterWindowEventArgs e) 
        {
            ShowAccountParameterWindowEventHandler handler = ShowMainMenuWindow;
            if (handler != null) 
            {
                handler(this, e);   
            }
        }
        protected virtual void OnShowAccountCreationWindow(ShowWindowEventArgs e) 
        {
            ShowWindowEventHandler handler = ShowAccountCreationWindow;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
        public void DisplayAccountCreationWindow()
        {
            ShowAccountCreation();
        }
        public void GetUserDetails(string username)
        {
            IList<UserModel> userDetails = new List<UserModel>();

            userDetails = _userService.GetUserDetails(username);

            _userModel.UserID = userDetails.Last().UserID;
            _userModel.Username = userDetails.Last().Username;
            _userModel.HashedPassword = userDetails.Last().HashedPassword;
            _userModel.TotalPoints = userDetails.Last().TotalPoints;
        }

        public void Login()
        {
            if (!(Username != null) && (Password != null)) 
            {
                ShowMessageDialog("Enter valid input.");
            }
            else 
            {
                GetUserDetails(Username);

                if (ValidateUserNameLogin(Username) != true)
                {
                    ShowMessageDialog("Username does not exist.");
                }
                else if (ValidatePasswordLogin(Username, Password) != true)
                {
                    ShowMessageDialog("Invalid password.");
                }
                else
                {
                    UserID = (int)_userModel.UserID;
                    Username = _userModel.Username;
                    TotalPoints = (int)_userModel.TotalPoints;

                    ShowMessageDialog("Account Login successful");

                    HideAccountLogin();
                    ShowMainMenu();
                }
            }
        }
        public bool ValidateUserNameLogin(string username)
        {
            if (_userModel.Username != null)
            {
                return true;
            }
            return false;
        }
        public bool ValidatePasswordLogin(string username, string password)
        {
            if (_userService.HashPassword(password) == _userModel.HashedPassword)
            {
                return true;
            }
            return false;
        }
    }
}
