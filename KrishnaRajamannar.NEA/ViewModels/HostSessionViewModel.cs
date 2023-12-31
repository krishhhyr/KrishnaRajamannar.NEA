using Azure;
using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class HostSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ShowMessageEventHandler ShowMessage;
        private readonly IServerService _serverService;
        private readonly ISessionService _sessionService;
        private readonly IClientService _clientService;
        private readonly UserConnectionService _userConnectionService;
        private readonly IQuizService _quizService;

        public int UserID;
        public string Username;
        IList<QuizModel> quizzes = new List<QuizModel>();

        public HostSessionViewModel(ISessionService sessionService, IQuizService quizService, 
            IServerService serverService, UserConnectionService userConnectionService, IClientService clientService)
        {
            _serverService = serverService;
            _sessionService = sessionService;
            _quizService = quizService;
            _userConnectionService = userConnectionService;
            _clientService = clientService;

            _userConnectionService.UserJoined += OnUserJoined;
            _userConnectionService.UserLeft += OnUserLeft;
            //_clientService.StartQuizEvent += _clientService_StartQuizEvent;
        }

        private void _clientService_StartQuizEvent(object sender, StartQuizEventArgs e)
        {
            throw new NotImplementedException();
        }

       

        #region Properties

        private List<string> _quizTitles;
        public List<string> QuizTitles 
        {
            get { return _quizTitles; }
            set 
            {
                _quizTitles = value;
                RaisePropertyChange("QuizTitles");
            }
        }
        private List<string> _endQuizConditions;
        public List<string> EndQuizConditions
        {
            get { return _endQuizConditions; }
            set
            {
                _endQuizConditions = value;
                RaisePropertyChange("EndQuizConditions");
            }
        }
        private string _selectedQuiz;
        public string SelectedQuiz 
        {
            get { return _selectedQuiz; }
            set 
            {
                _selectedQuiz = value;
                RaisePropertyChange("SelectedQuiz");
            }
        }
        private string _selectedCondition;
        public string SelectedCondition 
        {
            get { return _selectedCondition; }
            set 
            {
                _selectedCondition = value;
                RaisePropertyChange("SelectedCondition");
            }
        }
        private string _conditionValue;
        public string ConditionValue
        {
            get { return _conditionValue; }
            set
            {
                _conditionValue = value;
                RaisePropertyChange("ConditionValue");
            }
        }
        private int _sessionID;
        public int SessionID
        {
            get { return _sessionID; }
            set
            {
                _sessionID = value;
                RaisePropertyChange("SessionID");
            }
        }

        private ObservableCollection<UserSessionData> _users = new ObservableCollection<UserSessionData>();
        public ObservableCollection<UserSessionData> Users 
        {
            get { return _users; }
            set 
            {
                _users = value;
                RaisePropertyChange("Users");
            }
        }

        private string _numberOfUsersJoined;
        public string NumberOfUsersJoined 
        {
            get { return _numberOfUsersJoined; }
            set 
            {
                _numberOfUsersJoined = value;
                RaisePropertyChange("NumberOfUsersJoined");
            }
        }

        #endregion

        #region Events
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

        private void OnUserJoined(object sender, UserJoinedEventArgs e)
        {
            // Used to go back onto the UI thread
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate ()
            {
                _users.Add(e.UserSession);

            });

            UpdateNumberOfUsersJoined();
        }

        // this is not working for some reason?
        private void OnUserLeft(object sender, UserLeftEventArgs e)
        {
            // Used to go back onto the UI thread
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate ()
            {
                //_users.Remove();
                ;
            });

            UpdateNumberOfUsersJoined();

        }

        #endregion

        // Used to generate a random six digit session ID in which users will enter to join to.
        public int CreateSessionID()
        {
            Random random = new Random();

            int sessionID = 0;
            bool valid = false;
            while (valid == false)
            {
                sessionID = random.Next(100000, 1000000);
                // This checks whether the session ID has already been created and stored in the DB
                // If it hasn't, the newly created ID can be used. 
                if (_sessionService.IsSessionIDExist(sessionID) == false)
                {
                    SessionID = sessionID;
                    valid = true;
                }
            }
            return sessionID;
        }

        // Used to assign the methods which the host can select to end a multiplayer quiz.
        // This is assigned to the property of the class which is binded to the UI.
        public void AssignQuizConditions()
        {
            List<string> endQuizConditions = new List<string>();
            endQuizConditions.Add("Number of Questions");
            endQuizConditions.Add("Time Limit");

            _endQuizConditions = endQuizConditions;
        }

        // This retrieves the IP address of the host's machine 
        // Used to know which IP address other users should connect to for the multiplayer quiz. 
        private string GetIPAddress()
        {
            string IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            return IPAddress;
        }
        // Generates a random port number which is where the server will start on.
        private int GetPortNumber()
        {
            Random random = new Random();

            int portNumber = 0;
            bool valid = false;
            while (valid == false)
            {
                // Randomising between 49152 and 65535 is used as these ports are not assigned to anything.
                // Known as Dynamic Ports, they are used for temporary/private connections.
                portNumber = random.Next(49152, 65536);
                // Checks if the port number has not previously been generated and stored in the DB.
                if (_sessionService.IsPortNumberExist(portNumber) == false)
                {
                    valid = true;
                }
            }
            return portNumber;
        }

        public void GetQuizzes()
        {
            List<string> titlesOfQuizzes = new List<string>();

            // Retrieves all the quizzes by a host based on the host's userID.
            // Stores them as a list of objects.
            quizzes = _quizService.GetQuiz(UserID);

            // Adds the title of each quiz retrieved into a separate list.
            foreach (var quiz in quizzes)
            {
                titlesOfQuizzes.Add(quiz.QuizTitle);
            }

            // Binds the combo box with the list of quiz titles.
            _quizTitles = titlesOfQuizzes;
        }

        // This validates the input in which the quiz ends if a certain number of 
        // questions has been answered.
        // This checks if the input is below or equal to the number of questions
        // in the quiz selected by the host.
        private bool ValidateNumberOfQuestionsInput()
        {
            bool valid = false;

            foreach (var quiz in quizzes)
            {
                if ((SelectedQuiz == quiz.QuizTitle) && (int.Parse(ConditionValue) <= quiz.NumberOfQuestions))
                {
                    valid = true;
                    return valid;
                }
            }
            if (valid == false)
            {
                ShowMessageDialog("Number of Questions entered must be smaller or equal " +
                    "to the number of questions in the quiz selected");
                return valid;
            }

            return valid;
        }

        // This validates the input in which the quiz ends if a certain amount of 
        // time has passed.
        // This checks if the input is between 5 and 60 minutes.
        private bool ValidateTimeInput()
        {
            if ((int.Parse(ConditionValue) >= 5) && (int.Parse(ConditionValue) <= 60))
            {
                return true;
            }
            else
            {
                ShowMessageDialog("Invalid input. Enter a value between 5 and 60 minutes.");
                return false;
            }
        }

        // Used to insert the session data into the Session table in the database. 
        // This also calls a function to start the server for the TCP/IP connection.
        public bool CreateSession() 
        {
            bool valid = false;

            if (SelectedCondition == "Number of Questions")
            {
                valid = ValidateNumberOfQuestionsInput();
            }
            else
            {
                valid = ValidateTimeInput();
            }

            if (valid == true) 
            {
                string ipAddress = GetIPAddress();
                int portNumber = GetPortNumber();

                _sessionService.InsertSessionData(SessionID, SelectedQuiz, SelectedCondition, ConditionValue
                ,ipAddress, portNumber, 36);
                _serverService.StartServer(Username, ipAddress, portNumber);  
                
                return true;
            }
            return false;
        }

        // Used to retrieve the titles of the quizzes which have been created by the host. 
        // This is used so that the host can select a quiz to review with other users.

        private void UpdateNumberOfUsersJoined()
        {
            int numberOfUsers = _users.Count;

            if (numberOfUsers == 1)
            {
                NumberOfUsersJoined = $"({numberOfUsers} user has joined.)";
            }
            else
            {
                NumberOfUsersJoined = $"({numberOfUsers} users have joined.)";
            }

        }

        public void StartQuiz()
        {
            _serverService.SendMessageToClients("StartQuiz");
        }
    } 
}
