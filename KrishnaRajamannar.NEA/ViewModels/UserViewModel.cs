using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public UserService _userService { get;  set; }

        public UserViewModel()
        {
            _userService = new UserService();
             
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
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

        // This region represents the logic behind the AccountLogin XAML window.
        // It represents how users can log into the application.
        #region AccountLogin
        //check if hashed password from DB is same as new inputted hashed password

        // This function validates username and password entered by the user.
        // It returns true if the details entered are valid (i.e they exist in the database).
        // It returns false if the details are not valid.
        public string Login()
        {
            if ((ValidateUserName(Username) == true) && (ValidatePasswordLogin(Username,Password) == true))
            {
                return "Successful Account Login.";
            }
            return "Username or password do not match. Try again.";
        }
        // This function checks whether the password entered matches the password in the database.
        // It hashes the inputted password and checks against the hashed password in the database.
        public bool ValidatePasswordLogin(string username, string password) 
        {
            if (_userService.HashPassword(password) == _userService.GetPassword(username)) 
            {
                return true;
            }
            return false;
        }
        #endregion

        // This regions represents the logic behind the AccountCreation XAML window.
        // It represents how users can create an account.
        #region AccountCreation

        // This function calls other functions to help validate the username and password before the details are 
        // inserted into the database.
        public string Creation() 
        {
            if ((ValidateUserName(Username) == true) && (ValidatePasswordCreation(Password, RetypedPassword) == true))
            {
                //call services to create the account 
                _userService.CreateUser(Username, Password);

                return "Successful Account Creation.";
            }
            return "Username or Password is invalid. Username must between 4-15 characters and must be original. " +
                "Password must be between 8-15 characters and must have a number. The password entered and the retyped password" +
                "must match";
        }
        // This function validates the username by checking the username meets the length requirements
        // and it also checks whether the username exists already in the database.
        public bool ValidateUserName(string username) 
        {
            if (username == null) return false;

            if ((username.Length >= 4) && (username.Length <= 15) && (_userService.IsUserExists(username) == false))
            {
                return true;
            }
            return false;
        }
        // This function validates the password that has been entered by the username for an account creation.
        // It calls other functions to check whether the password has a number and if the passwords entered match.
        // It also checks the length of the password itself.
        // It returns true if the password meets the requirement, false if it doesn't.
        public static bool ValidatePasswordCreation(string password, string retypedPassword) 
        {
            if (password == null) return false;

            if ((password.Length > 8) && (password.Length < 15) && (IsPasswordContainsNumber(password) == true) 
                && (IsPasswordsMatch(password, retypedPassword) == true))
            {
                return true;
            }
            return false;
        }
        //A function which checks if a password contains a number or not.
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
        // A function which checks if the original password entered matches the retyped password entered.
        private static bool IsPasswordsMatch(string initialPassword, string retypedPassword) 
        {
            if (initialPassword == retypedPassword)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        #endregion
    }
}
