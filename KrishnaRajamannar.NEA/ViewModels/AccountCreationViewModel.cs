using KrishnaRajamannar.NEA.Events;
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
    public class AccountCreationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event ShowMessageEventHandler ShowMessage;

        //public event ShowWindowEventHandler ShowAccountLoginWindow;

        public event HideWindowEventHandler HideAccountCreationWindow;

        private readonly IUserService _userService;

        public AccountCreationViewModel(IUserService userService)
        {
            _userService = userService;
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
        private string _retypedPassword;
        public string RetypedPassword 
        {
            get { return _retypedPassword;}
            set 
            {
                _retypedPassword = value;
                RaisePropertyChange("RetypedPassword");
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
        private void HideAccountCreation()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideAccountCreationWindow(args);
        }
        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnHideAccountCreationWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideAccountCreationWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void Creation()
        {
            if (!(Username != null) && (Password != null) && (RetypedPassword != null))
            {
                ShowMessageDialog("Enter valid input.");
            }
            else 
            {
                if ((ValidateUsernameCreation(Username) == true) && (ValidatePasswordCreation(Password, RetypedPassword) == true))
                {
                    //call services to create the account 
                    _userService.CreateUser(Username, Password);

                    ShowMessageDialog("Account Creation Successful");

                    HideAccountCreation();
                }
            }


        }
        public void CloseAccountCreationWindow() 
        {
            HideAccountCreation();
        }
        public bool ValidateUsernameCreation(string username)
        {
            if (username == null)
            {
                return false;
            } 

            if (!(username.Length >= 4) && (username.Length <= 15))
            {
                ShowMessageDialog("Username must be between 4-15 characters.");
            }
            else if (!(_userService.IsUserExists(username) == true))
            {
                ShowMessageDialog("Username already exists.");
            }
            else 
            {
                return true;
            }
            return false;
        }
        public bool ValidatePasswordCreation(string password, string retypedPassword)
        {
            if (password == null) 
            {
                return false;
            }

            if (!(password.Length > 8) && (password.Length < 15))
            {
                ShowMessageDialog("Password must be between 8-15 characters.");
                return false;
            }
            else if (!(IsPasswordContainsNumber(password) == true))
            {
                ShowMessageDialog("Password must have a number.");
                return false;
            }
            else if (!(IsPasswordsMatch(password, retypedPassword)))
            {
                ShowMessageDialog("Passwords entered do not match.");
                return false;
            }
            else 
            {
                return true;
            }
        }
        private static bool IsPasswordContainsNumber(string password)
        {
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            foreach (char character in password)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (character == digits[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static bool IsPasswordsMatch(string initialPassword, string retypedPassword)
        {
            if (initialPassword != retypedPassword)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
