using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Services;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    // Inherites the INotifyPropertyChanged interface
    public class AccountCreationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event HideWindowEventHandler HideAccountCreationWindow;
        private readonly IUserService _userService;
        public AccountCreationViewModel(IUserService userService)
        {
            _userService = userService;
        }

        // Binds with the UI to recieve the username input directly 
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
        // Binds with the UI to recieve the password input  
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
        // Binds with the UI to recieve the retyped password input  
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
        // Binds with the UI to display the error messages
        // Used to display whether the account creation was successful or not
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

        // Used to notify the UI for any changes in the values of properties
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public void CloseAccountCreationWindow()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideAccountCreationWindow(args);
        }

        // An event which hides the account creation window if the back button is pressed
        // of if the user successfully creates an account
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
            // Checks if the username, password and retyped password is not empty
            if (!(Username != null) && (Password != null) && (RetypedPassword != null))
            {
                Message = "Enter valid input.";
            }
            else 
            {
                if ((ValidateUsernameCreation() == true) && (ValidatePasswordCreation() == true))
                {
                    //Stores the user data into the database
                    _userService.CreateUser(Username, Password);

                    Message = "Account Creation Successful";

                    CloseAccountCreationWindow();
                }
            }


        }
    
        private bool ValidateUsernameCreation()
        {
            // Checks if the username inputted is between 4 -15 characters
            if (!(Username.Length >= 4) && (Username.Length <= 15))
            {
                Message = "Username must be between 4-15 characters.";
            }
            // Checks if the username has not already been created and 
            // stored in the DB
            else if ((_userService.IsUserExists(Username) != false))
            {
                Message = "Username already exists.";
            }
            else 
            {
                return true;
            }
            return false;
        }
        private bool ValidatePasswordCreation()
        {
            if (!(Password.Length > 8) && (Password.Length < 15))
            {
                Message = "Password must be between 8-15 characters.";
                return false;
            }
            else if (IsPasswordContainsNumber(Password) != true)
            {
                Message = "Password must have a number.";
                return false;
            }
            else if (!(IsPasswordsMatch(Password, RetypedPassword)))
            {
                Message = "Passwords entered do not match.";
                return false;
            }
            else 
            {
                return true;
            }
        }
        // Checks if the password has a number 
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
        // Checks if the passwords inputted match each other 
        private static bool IsPasswordsMatch(string initialPassword, string retypedPassword)
        {
            if (initialPassword != retypedPassword)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
