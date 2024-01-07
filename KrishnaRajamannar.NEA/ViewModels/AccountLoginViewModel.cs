using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System.Collections.Generic;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    // Class inherits INotifyPropertyChanged interface which is used to notify users that the values
    // of properties have changed
    public class AccountLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ShowAccountParameterWindowEventHandler ShowMainMenuWindow;
        public event ShowWindowEventHandler ShowAccountCreationWindow;
        public event HideWindowEventHandler HideAccountLoginWindow;
        // An interface which uses UserService which handles action relating to the UserDetails table in DB
        private readonly IUserService _userService;
        private readonly UserModel _userModel;
        public AccountCreationViewModel AccountCreationViewModel;

        public int UserID;
        public int TotalPoints;

        public AccountLoginViewModel(IUserService userService, UserModel userModel)
        {
            _userService = userService;
            _userModel = userModel;

            AccountCreationViewModel = App.ServiceProvider.GetService(typeof(AccountCreationViewModel)) as AccountCreationViewModel;
        }

        // Binds the username with the UI to retrieve the username from the UI
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
        // Binds the password with the UI to retrieve the username from the UI
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

        // Binds the message label with the UI to notify the user
        // if the login was successful or not
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

        // Used to notify the UI if any property has been changed
        // Used for data binding
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        public void ShowAccountCreation()
        {
            ShowWindowEventArgs args = new ShowWindowEventArgs();
            args.IsShown = true;
            OnShowAccountCreationWindow(args);

        }
        private void HideAccountLogin() 
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideAccountLoginWindow(args);
        }
        // Used to show the main menu after a user has logged in
        // Passes the user details on to be displayed in the main menu window
        private void ShowMainMenu() 
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = UserID;
            args.Username = Username;
            args.TotalPoints = TotalPoints;
            OnShowMainMenuWindow(args);
        }
        // Raises an event to close the Account Login window
        protected virtual void OnHideAccountLoginWindow(HideWindowEventArgs e) 
        {
            HideWindowEventHandler handler = HideAccountLoginWindow;
            if (handler != null) 
            {
                handler(this, e);
            }
        }

        // Raises an event to show the Main Menu window
        protected virtual void OnShowMainMenuWindow(ShowAccountParameterWindowEventArgs e) 
        {
            ShowAccountParameterWindowEventHandler handler = ShowMainMenuWindow;
            if (handler != null) 
            {
                handler(this, e);   
            }
        }

        // Raises an event to show the Account Creation window
        protected virtual void OnShowAccountCreationWindow(ShowWindowEventArgs e) 
        {
            ShowWindowEventHandler handler = ShowAccountCreationWindow;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
        public void GetUserDetails(string username)
        {
            IList<UserModel> userDetails = new List<UserModel>();

            userDetails = _userService.GetUserDetails(username);

            _userModel.UserID = userDetails[0].UserID;
            _userModel.Username = userDetails[0].Username;
            _userModel.HashedPassword = userDetails[0].HashedPassword;
            _userModel.TotalPoints = userDetails[0].TotalPoints;

            // Assigns information to variables to pass onto other windows
            UserID = (int)_userModel.UserID;
            Username = _userModel.Username;
            TotalPoints = (int)_userModel.TotalPoints;
        }

        // Used to validate the overall login for users 
        // Calls an event to show the main menu in the case of successful logins
        // Displays error messages
        public void Login()
        {
            // Checks if the username or password does not have an empty value
            if ((((Username != "") || (Password != "")) || ((Username != null) || (Password != null)))) 
            {
                GetUserDetails(Username);
                if ((ValidateUserNameLogin(Username) == true) && (ValidatePasswordLogin(Password) == true))
                {
                    // Retrieves all the user information about the user
                    Message = "Account Login successful";
                    // Procedures which hide the Account Login window and display the Main Menu window
                    ShowMainMenu();
                    HideAccountLogin();
                }
            }
            else 
            {
                Message = "Enter a valid input.";
            }
        }
        // Checks if the username exists in the database
        public bool ValidateUserNameLogin(string username)
        {
            if (_userService.IsUserExists(username) != true)
            {
                Message = "Username does not exist";
                return false;
            }
            return true;
        }

        // Used to check if the password matches the hashed password found in the database
        // Hashes the entered password and checks against the password found in the DB
        public bool ValidatePasswordLogin(string password)
        {
            if ((Password != null) || (Password != "")) 
            {
                if (_userService.HashPassword(password) != _userModel.HashedPassword)
                {
                    Message = "Invalid password";
                    return false;
                }
                
                return true;
            }
            return false;
            
        }
    }
}
