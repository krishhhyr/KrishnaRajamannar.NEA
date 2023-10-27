using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KrishnaRajamannar.NEA.ViewModels
{

    // INotifyPropertyChanged is used to indicate the changes being made to the textboxes that users input data into. 
    // It's used to update the changed data in the local variables in this class from the UI code.
    // This whole process is used for data binding. 
    public class UserViewModel : INotifyPropertyChanged
    {

        // This instantiates all the classes and interfaces which are used in this class

        private readonly IUserService _userService;

        private readonly UserModel _userModel;

        private readonly AccountCreation _accountCreation;
        private readonly AccountLogin _accountLogin;

        private readonly MainMenu _mainMenu;

        // Used to show messages to UI.
        public event ShowMessageEventHandler ShowMessage;


        public UserViewModel(IUserService userService, MainMenuViewModel mainMenuViewModel, UserModel userModel)
        {
            _userService = userService;
            _userModel = userModel;

            // Not allowed!
            _accountCreation = new AccountCreation(this);
            //_accountLogin = new AccountLogin(this); 

            _mainMenu = new MainMenu(this ,mainMenuViewModel);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        // This region represents the variables which will be affected by data binding.
        // It retrieves the data from the UI elements that have data binding enabled.
        // It then assigns that data to a local variable. 

        #region DataBindingVariables

        private string _username;
        public string Username
        {
            get 
            {
                return _username; 
            }
            set
            {
                _username = value;
                // This is used to link the data retrieved from the username textbox in the account registration to the UserViewModel class
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

        #endregion 

        public int UserID;
        public int TotalPoints;

        #region Windows 

        public void ShowAccountLogin()
        {
            if (_accountLogin != null)
            {
                _accountLogin.Visibility = Visibility.Visible;

                _accountCreation.Visibility = Visibility.Collapsed;
            }
        }

        public void ShowAccountCreation()
        {
            if (_accountCreation != null)
            {
                _accountCreation.Visibility = Visibility.Visible;

                _accountLogin.Visibility = Visibility.Hidden;
            }
        }

        public void ShowMainMenu()
        {
            if (_mainMenu != null)
            {
                _mainMenu.LoadData();

                _mainMenu.Visibility = Visibility.Visible;

                _accountLogin.Visibility= Visibility.Hidden;
            }
        }

        #endregion

        #region AccountLogin

        public void GetUserDetails(string username)
        {
            IList<UserModel> userDetails = new List<UserModel>();

            userDetails = _userService.GetUserDetails(username);

            _userModel.UserID = userDetails.Last().UserID;
            _userModel.Username = userDetails.Last().Username;
            _userModel.HashedPassword = userDetails.Last().HashedPassword;
            _userModel.TotalPoints = userDetails.Last().TotalPoints;
        }

        // This function validates username and password entered by the user.
        // It returns true if the details entered are valid (i.e they exist in the database).
        // It returns false if the details are not valid.
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

                    //MessageBox.Show("Successful Account Login.");

                    ShowMessageEventArgs args = new ShowMessageEventArgs();
                    args.Message = "Successful Account Login.";
                    OnShowMessage(args);

                    ShowMainMenu();

                    return true;
                }
                else 
                {
                    //MessageBox.Show("Username and password do not match. Try again.");
                    ShowMessageEventArgs args = new ShowMessageEventArgs();
                    args.Message = "Username and password do not match. Try again.";
                    OnShowMessage(args);
                    return false;
                }
            }
            else 
            {

                ShowMessageDialog("No details entered");
                return false;
            }
        }

        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // This function checks whether the password entered matches the password in the database.
        // It hashes the inputted password and checks against the hashed password in the database.
        public bool ValidatePasswordLogin(string username, string password) 
        {
            if (_userService.HashPassword(password) == _userModel.HashedPassword) 
            {
                return true;
            }
            return false;
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


        #endregion

        #region AccountCreation

        // This function calls other functions to help validate the username and password before the details are 
        // inserted into the database.
        public bool Creation() 
        {
            if ((Username != null) || (Password != null) || (RetypedPassword != null))
            {
                if ((ValidateUsernameCreation(Username) == true) && (ValidatePasswordCreation(Password, RetypedPassword) == true))
                {
                    //call services to create the account 
                    _userService.CreateUser(Username, Password);

                    MessageBox.Show("Successful Account Creation.");

                    return true;
                }
            }
            return false; 
        }


        // This function validates the username by checking the username meets the length requirements
        // and it also checks whether the username exists already in the database.
        public bool ValidateUsernameCreation(string username) 
        {
            if (username == null) return false;

            if ((username.Length >= 4) && (username.Length <= 15) && (_userService.IsUserExists(username) == false))
            {
                return true;
            }
            MessageBox.Show("The username must original and must be between 4 and 15 characters.");

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

            MessageBox.Show("The password must be between 8 and 15 characters");

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

            MessageBox.Show("The password must contain a number.");

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
                MessageBox.Show("The passwords typed do not match.");

                return false;
            }
        }

        private void ShowMessageDialog(string message)
        {
            ShowMessageEventArgs args = new ShowMessageEventArgs();
            args.Message = message;
            OnShowMessage(args);
        }

        #endregion
    }
}
