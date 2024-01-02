using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Database;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ServerSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event QuestionRecievedEventHandler TextQuestionRecieved;
        public event QuestionRecievedEventHandler MultipleChoiceQuestionRecieved;
        private readonly IServerService _serverService;
        private readonly ISessionService _sessionService;
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService; 

        public int UserID;
        public string Username;
        IList<QuizModel> Quizzes = new List<QuizModel>();
        IList<QuestionModel> Questions = new List<QuestionModel>();
        public int NumberOfQuestions; 

        public ServerSessionViewModel(IServerService serverService, ISessionService sessionService, IQuizService quizService, IQuestionService questionService)
        {
            _serverService = serverService;            
            _sessionService = sessionService;
            _quizService = quizService;
            _questionService = questionService;
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

        private string _question;
        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                RaisePropertyChange("Question");
            }
        }

        private string _correctAnswer;
        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                _correctAnswer = value;
                RaisePropertyChange("CorrectAnswer");
            }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                _option1 = value;
                RaisePropertyChange("Option1");
            }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set
            {
                _option2 = value;
                RaisePropertyChange("Option2");
            }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set
            {
                _option3 = value;
                RaisePropertyChange("Option3");
            }
        }

        private string _option4;
        public string Option4
        {
            get { return _option4; }
            set
            {
                _option4 = value;
                RaisePropertyChange("Option4");
            }
        }

        private string _option5;
        public string Option5
        {
            get { return _option5; }
            set
            {
                _option5 = value;
                RaisePropertyChange("Option5");
            }
        }

        private string _option6;
        public string Option6
        {
            get { return _option6; }
            set
            {
                _option6 = value;
                RaisePropertyChange("Option6");
            }
        }

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        #endregion

        #region Events
        private void ShowTextQuestion()
        {
            QuestionRecievedEventArgs args = new QuestionRecievedEventArgs();
            OnShowTextQuestion(args);

        }

        protected virtual void OnShowTextQuestion(QuestionRecievedEventArgs e)
        {
            QuestionRecievedEventHandler handler = TextQuestionRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ShowMultipleChoiceQuestion()
        {
            QuestionRecievedEventArgs args = new QuestionRecievedEventArgs();
            OnShowMultipleChoiceQuestion(args);

        }

        protected virtual void OnShowMultipleChoiceQuestion(QuestionRecievedEventArgs e)
        {
            QuestionRecievedEventHandler handler = MultipleChoiceQuestionRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region SessionConfiguration

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
            Quizzes = _quizService.GetQuiz(UserID);

            // Adds the title of each quiz retrieved into a separate list.
            foreach (var quiz in Quizzes)
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

            foreach (var quiz in Quizzes)
            {
                if ((SelectedQuiz == quiz.QuizTitle) && (int.Parse(ConditionValue) <= quiz.NumberOfQuestions))
        {
                    valid = true;
                    return valid;
                }
            }
            if (valid == false)
            {
                Message = "Invalid input. Enter a value that matches the number of questions in quiz.";
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
                Message = "Invalid input. Enter a value between 5 and 60 minutes.";
                return false;
            }
        }

        // Used to insert the session data into the Session table in the database. 
        // This also calls a function to start the server for the TCP/IP connection.
        public bool StartSession()
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
                , ipAddress, portNumber, 36);
                _serverService.StartServer(Username, ipAddress, portNumber);
                Message = "Session started";

                return true;
                    }
            return false;
                }

        #endregion

        // Sends the first question + time to answer? to all clients 
        public void StartQuiz()
        {
            Message = "Quiz Started";

            foreach (QuizModel quiz in Quizzes) 
            {
                if (quiz.QuizTitle == SelectedQuiz) 
                {
                    Questions = _questionService.GetQuestions(quiz.QuizID); 
                }
            }

            NumberOfQuestions = Questions.Count;

            QuestionModel firstQuestion = Questions.First();
            string questionData = JsonSerializer.Serialize<QuestionModel>(firstQuestion);
            _serverService.SendDataToClients(questionData, "SendQuestion");
            DisplayQuestion(firstQuestion);

            }

        public QuizModel RandomiseQuiz(QuizModel quiz)
        {
            return quiz;
        }

        private void DisplayQuestion(QuestionModel question) 
        {
            if (question.Option1 != null)
            {
                ShowMultipleChoiceQuestion();
            }
            else 
            {
                ShowTextQuestion();
            }
            AssignQuestionValues(question);
        }
        private void AssignQuestionValues(QuestionModel questionData)
        {
            Question = questionData.Question;
            Option1 = questionData.Option1;
            Option2 = questionData.Option2;
            Option3 = questionData.Option3;
            Option4 = questionData.Option4;
            Option5 = questionData.Option5;
            Option6 = questionData.Option6;
        }


        public void StopServer() 
        {
           _serverService.StopServer();
        }
    }
}
