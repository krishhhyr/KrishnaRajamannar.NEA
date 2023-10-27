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
        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;
            if (handler != null)
            {
                handler(this, e);
            }
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

        public bool Login()
        {
            if ((Username != null) || (Password != null))
            {
                GetUserDetails(Username);

                if ((ValidateUserNameLogin(Username) == true) && (ValidatePasswordLogin(Username, Password) == true))
                {
                    UserID = (int)_userModel.UserID;
                    Username = _userModel.Username;
                    TotalPoints = (int)_userModel.TotalPoints;

                    ShowMessageDialog("Account Login successful");
                    return true;
                }
                else
                {
                    ShowMessageDialog("Username and password do no match. Try again.");
                    return false;
                }
            }
            else
            {

                ShowMessageDialog("No details entered");
                return false;
            }
        }
        public bool ValidateUserNameLogin(string username)
        {
            if (_userModel.Username != null)
            {
                return true;
            }
            MessageBox.Show("This username does not exist.");
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
