using Azure;
using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class HostSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ShowMessageEventHandler ShowMessage;
        private readonly IServerService _serverService;
        private readonly ISessionService _sessionService;
        private readonly IQuizService _quizService;

        public int UserID;
        IList<QuizModel> quizzes = new List<QuizModel>();

        public HostSessionViewModel(ISessionService sessionService, IQuizService quizService, IServerService serverService)
        {
            _serverService = serverService;
            _sessionService = sessionService;
            _quizService = quizService;
        }
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
        private int _endQuizConditionInput;
        public int EndQuizConditionInput
        {
            get { return _endQuizConditionInput; }
            set
            {
                _endQuizConditionInput = value;
                RaisePropertyChange("ConditionInput");
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
        // Used to insert the session data into the Session table in the database. 
        // This also calls a function to start the server for the TCP/IP connection.
        public void CreateSession(int quizID) 
        {
            int sessionID = CreateSessionID();
            //SessionID = sessionID;
            //string IPAddress = GetIPAddress();
            //int portNumber = GetPortNumber();

            //string ipAddress = GetIPAddress();
            //int portNumber = GetPortNumber();

            string ipAddress = "192.168.0.65";
            int portNumber = 60631;


            //_sessionService.InsertSessionData(CreateSessionID(), ipAddress, portNumber, quizID);
            _serverService.StartServer(ipAddress, portNumber);
        }
        // This retrieves the IP address of the host's machine 
        // Used to know which IP address other users should connect to for the multiplayer quiz. 
        public string GetIPAddress() 
        {
            string IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            return IPAddress;
        }
        // Generates a random port number which is where the server will start on.
        public int GetPortNumber() 
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
        // Used to retrieve the titles of the quizzes which have been created by the host. 
        // This is used so that the host can select a quiz to review with other users.
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
        public IList<QuizModel>? GetQuestions(int quizID) 
        {
            return null;
        }

        // This validates the input in which the quiz ends if a certain number of 
        // questions has been answered.
        // This checks if the input is below or equal to the number of questions
        // in the quiz selected by the host.
        public bool ValidateNumberOfQuestionsInput(string quizTitle) 
        {
            bool valid = false;

            if (EndQuizConditionInput is int)
            {
                foreach (var quiz in quizzes)
                {
                    if ((quizTitle == quiz.QuizTitle) && (EndQuizConditionInput <= quiz.NumberOfQuestions))
                    {
                        GetQuestions(quiz.QuizID);
                        valid = true;
                        return true;
                    }
                }
                if (valid == false)
                {
                    ShowMessageDialog("Invalid input.");
                    return false;
                }
            }
            else 
            {
                ShowMessageDialog("Invalid data format.");
                return false;
            }
            return false;
        }

        // This validates the input in which the quiz ends if a certain amount of 
        // time has passed.
        // This checks if the input is between 5 and 60 minutes.
        public bool ValidateTimeInput(string quizTitle)
        {
            if ((EndQuizConditionInput >= 5) && (EndQuizConditionInput <= 60))
            {
                foreach (var quiz in quizzes)
                {
                    if (quizTitle == quiz.QuizTitle)
                    {
                        GetQuestions(quiz.QuizID);
                        return true;
                    }
                }
            }
            else 
            {
                ShowMessageDialog("Invalid input. Enter a value between 5 and 60 minutes.");
                return false;
            }
            return false;
        }
    } 
}
