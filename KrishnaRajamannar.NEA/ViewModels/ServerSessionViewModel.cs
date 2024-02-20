using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Models.DTO;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Database;
using KrishnaRajamannar.NEA.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ServerSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event QuestionRecievedEventHandler TextQuestionRecieved;
        public event QuestionRecievedEventHandler MultipleChoiceQuestionRecieved;
        public event ShowAccountParameterWindowEventHandler ShowMultipleReviewQuizFeedback;
        public event TimerEventHandler AnswerTimerFinished;
        private readonly IServerService _serverService;
        private readonly ISessionService _sessionService;
        private readonly IQuizService _quizService;
        private readonly IMultiplayerReviewQuizService _multiplayerReviewQuizService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public int UserID;
        public string Username;
        public int TotalPoints;

        private int SessionTimeLimit = 0;
        IList<QuizModel> Quizzes = new List<QuizModel>();
        IList<QuestionModel> Questions = new List<QuestionModel>();
        QuestionModel CurrentQuestion = new QuestionModel();
        int Counter = 0;
        public int NumberOfQuestions;
        public int QuestionNumber = 0;
        private int QuizID;
        private DispatcherTimer answerTimer;
        private TimeSpan AnswerTime;
        private DispatcherTimer sessionTimer;
        private TimeSpan SessionTime;

        public MultipleReviewQuizFeedbackViewModel MultipleReviewQuizFeedbackViewModel;

        public ServerSessionViewModel(IServerService serverService, ISessionService sessionService, IQuizService quizService,
            IQuestionService questionService, IUserService userService, IMultiplayerReviewQuizService multiplayerReviewQuizService)
        {
            answerTimer = new DispatcherTimer();
            sessionTimer = new DispatcherTimer();

            _serverService = serverService;
            _sessionService = sessionService;
            _quizService = quizService;
            _questionService = questionService;
            _userService = userService;
            _multiplayerReviewQuizService = multiplayerReviewQuizService;

            _serverService.ProcessClientResponse += OnProcessClientResponse;
            answerTimer.Tick += AnswerTimer_Tick;
            sessionTimer.Tick += SessionTimer_Tick;

            MultipleReviewQuizFeedbackViewModel = App.ServiceProvider.GetService(typeof(MultipleReviewQuizFeedbackViewModel)) as MultipleReviewQuizFeedbackViewModel;
        }

        private void OnProcessClientResponse(object sender, ProcessClientResponseEventArgs e)
        {
            ProcessClientResponse(e.ClientResponse);
        }

        #region Properties

        private string _timeOfSession;
        public string TimeOfSession
        {
            get { return _timeOfSession; }
            set
            {
                _timeOfSession = value;
                RaisePropertyChange("TimeOfSession");
            }
        }

        private int _numberOfQuestion;
        public int NumberOfQuestion
        {
            get { return _numberOfQuestion; }
            set
            {
                _numberOfQuestion = value;
                RaisePropertyChange("NumberOfQuestion");
            }
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

        private string _validAnswerMessage;
        public string ValidAnswerMessage
        {
            get { return _validAnswerMessage; }
            set
            {
                _validAnswerMessage = value;
                RaisePropertyChange("ValidAnswerMessage");
            }
        }

        private List<UserSessionData> _joinedUsers = new List<UserSessionData>();
        public List<UserSessionData> JoinedUsers
        {
            get { return _joinedUsers; }
            set
            {
                _joinedUsers = value;
                RaisePropertyChange("JoinedUsers");
            }
        }

        private int _numberOfUsersJoined;
        public int NumberOfUsersJoined
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

        private string _textanswerInput;
        public string TextAnswerInput
        {
            get { return _textanswerInput; }
            set
            {
                _textanswerInput = value;
                RaisePropertyChange("TextAnswerInput");
            }
        }

        private string _multipleChoiceanswerInput;
        public string MultipleChoiceAnswerInput
        {
            get { return _multipleChoiceanswerInput; }
            set
            {
                _multipleChoiceanswerInput = value;
                RaisePropertyChange("MultipleChoiceAnswerInput");
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

        private string _remainingAnswerTimeLimit;
        public string RemainingAnswerTimeLimit
        {
            get { return _remainingAnswerTimeLimit; }
            set
            {
                _remainingAnswerTimeLimit = value;
                RaisePropertyChange("RemainingAnswerTimeLimit");
            }
        }

        private string _remainingSessionTimeLimit;
        public string RemainingSessionTimeLimit
        {
            get { return _remainingSessionTimeLimit; }
            set
            {
                _remainingSessionTimeLimit = value;
                RaisePropertyChange("RemainingSessionTimeLimit");
            }
        }

        private string _answerTimeLimit;
        public string AnswerTimeLimit
        {
            get { return _answerTimeLimit; }
            set
            {
                _answerTimeLimit = value;
                RaisePropertyChange("AnswerTimeLimit");
            }
        }

        private int _numberOfPointsGained;
        public int NumberOfPointsGained
        {
            get { return _numberOfPointsGained; }
            set
            {
                _numberOfPointsGained = value;
                RaisePropertyChange("NumberOfPointsGained");
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

        protected virtual void OnShowTextQuestion(QuestionRecievedEventArgs e)
        {
            QuestionRecievedEventHandler handler = TextQuestionRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShowMultipleChoiceQuestion(QuestionRecievedEventArgs e)
        {
            QuestionRecievedEventHandler handler = MultipleChoiceQuestionRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDisableAnsweringQuestion(TimerEventArgs e)
        {
            TimerEventHandler handler = AnswerTimerFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShowMultipleReviewQuizFeedback(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowMultipleReviewQuizFeedback;
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

        // Used to retrieve the Quiz ID of the quiz that was selected by the host.
        private void GetQuizIDOfSelectedQuiz()
        {
            foreach (var quiz in Quizzes)
            {
                if (SelectedQuiz == quiz.QuizTitle)
                {
                    QuizID = quiz.QuizID;
                }
            }
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
                    QuizID = quiz.QuizID;
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
            if (ConditionValue != null)
            {
                if ((int.Parse(ConditionValue) >= 5) && (int.Parse(ConditionValue) <= 60))
                {
                    return true;
                }
                Message = "Invalid input. Enter a value between 5 and 60 minutes.";
                return false;
            }
            Message = "Invalid input. Enter a value between 5 and 60 minutes.";
            return false;
        }

        // Used to insert the session data into the Session table in the database. 
        // This also calls a function to start the server for the TCP/IP connection.
        public bool StartSession()
        {
            bool valid = false;

            if (SelectedQuiz != null)
            {
                GetQuizIDOfSelectedQuiz();

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
                    , ipAddress, portNumber, QuizID);
                    _serverService.StartServer(Username, ipAddress, portNumber);
                    Message = "Session Started.";

                    return true;
                }
                return false;
            }
            Message = "Invalid Input. Enter a valid quiz to review.";
            return false;
        }

        #endregion

        // Sends the first question + time to answer? to all clients 

        private void ProcessClientResponse(ClientResponse response)
        {
            if (response != null)
            {
                if (!string.IsNullOrEmpty(response.Data))
                {
                    UserSessionData userData = JsonSerializer.Deserialize<UserSessionData>(response.UserData);
                    ValidateClientAnswer(response.Data, userData.UserID, userData.Username, userData.TotalPoints);

                    System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate ()
                    {
                        SendNextQuestion();
                    });
                }
            }
        }

        public void LoadData(ClientResponse response)
        {
            if (response != null)
            {
                if (!string.IsNullOrEmpty(response.Data))
                {
                    SessionData data = JsonSerializer.Deserialize<SessionData>(response.Data);
                    if (data != null)
                    {
                        if (data.UserSessions.Any())
                        {

                            JoinedUsers.Clear();

                            //_userSessionData.AddRange(data.UserSessions);
                            JoinedUsers = data.UserSessions.ToList();

                            NumberOfUsersJoined = JoinedUsers.Count;
                        }
                    }
                }
            }

        }

        public void StartQuiz()
        {
            Message = "Quiz Started.";

            if (SelectedCondition == "Time Limit")
            {
                AssignSessionTimeValues();
                sessionTimer.Start();
            }

            foreach (QuizModel quiz in Quizzes)
            {
                if (quiz.QuizTitle == SelectedQuiz)
                {
                    Questions = _questionService.GetQuestions(quiz.QuizID);
                }
            }

            QuestionModel firstQuestion = Questions[QuestionNumber];
            CurrentQuestion = firstQuestion;
            string questionData = JsonSerializer.Serialize<QuestionModel>(firstQuestion);
            _serverService.SendDataToClients(questionData, "SendQuestion");
            DisplayQuestion(firstQuestion);
            NumberOfQuestion = QuestionNumber + 1;
        }

        private void DisplayQuestion(QuestionModel question)
        {

            if (question.Option1 != "")
            {
                QuestionRecievedEventArgs args = new QuestionRecievedEventArgs();
                OnShowMultipleChoiceQuestion(args);
            }
            else
            {
                QuestionRecievedEventArgs args = new QuestionRecievedEventArgs();
                OnShowTextQuestion(args);
            }
            AssignQuestionValues(question);
            AssignAnswerTimeValues(question.Duration);
            answerTimer.Start();
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

        private void AssignSessionTimeValues()
        {
            int sessionTime = int.Parse(ConditionValue) * 60;
            SessionTime = TimeSpan.FromSeconds(sessionTime);
            sessionTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void SessionTimer_Tick(object? sender, EventArgs e)
        {
            if (SessionTime == TimeSpan.Zero)
            {
                answerTimer.Stop();
                Message = "Time for Session is up!";
                ShowMultipleReviewQuizFeedbackWindow();
            }
            else
            {
                SessionTime = SessionTime.Add(TimeSpan.FromSeconds(-1));
                RemainingSessionTimeLimit = SessionTime.TotalSeconds.ToString();
            }
        }

        private void ShowMultipleReviewQuizFeedbackWindow() 
        {
            if (!(Counter > 0))
            {
                ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
                args.IsShown = true;
                args.UserID = UserID;
                OnShowMultipleReviewQuizFeedback(args);
            }
            else 
            {
                Counter++;
            }
        }

        private void AssignAnswerTimeValues(int answerTime)
        {
            AnswerTimeLimit = answerTime.ToString();
            AnswerTime = TimeSpan.FromSeconds(answerTime);
            answerTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void AnswerTimer_Tick(object? sender, EventArgs e)
        {
            if (AnswerTime == TimeSpan.Zero)
            {
                answerTimer.Stop();
                SendAnswer();
            }
            else
            {
                AnswerTime = AnswerTime.Add(TimeSpan.FromSeconds(-1));
                RemainingAnswerTimeLimit = AnswerTime.Seconds.ToString();
            }
        }

        public void SendAnswer()
        {
            TimerEventArgs args = new TimerEventArgs();
            OnDisableAnsweringQuestion(args);
            Message = "Times up! Validating Response...";
            ValidateServerAnswer();
        }

        public void ValidateServerAnswer()
        {
            if ((CurrentQuestion.Answer == TextAnswerInput) || (CurrentQuestion.Answer == MultipleChoiceAnswerInput))
            {
                NumberOfPointsGained = NumberOfPointsGained + CurrentQuestion.NumberOfPoints;
                TotalPoints = TotalPoints + CurrentQuestion.NumberOfPoints;
                ValidAnswerMessage = $"Correct Answer. {CurrentQuestion.NumberOfPoints} points have been awarded!";
                Message = "Correct Answer!";
                _userService.UpdatePoints(UserID, TotalPoints);
                _multiplayerReviewQuizService.InsertMultiplayerQuizFeedbackData(SessionID, UserID, QuizID, CurrentQuestion.Question, CurrentQuestion.Answer, true);
            }
            else
            {
                Message = "Incorrect Answer!";
                ValidAnswerMessage = $"Answer was {CurrentQuestion.Answer}. 0 points have been awarded!";
                _multiplayerReviewQuizService.InsertMultiplayerQuizFeedbackData(SessionID, UserID, QuizID, CurrentQuestion.Question, CurrentQuestion.Answer, false);
            }

        }

        private bool CheckEndQuiz()
        {
            if ((SelectedCondition == "Number of Questions") && (QuestionNumber >= int.Parse(ConditionValue)))
            {
                Message = "No more questions left to review.";
                return true;
            }
            //else if ((SelectedCondition == "Time Limit") && (sessionTime == ConditionValue))
            //{
            //    Message = "Session Time has been reached.";
            //    return true;
            //}
            else
            {
                return false;
            }
        }

        private void SendNextQuestion()
        {
            QuestionNumber++;
            NumberOfQuestion = QuestionNumber + 1;
            if (NumberOfQuestion > Questions.Count)
            {
                Message = "No more questions to review.";

                ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
                args.IsShown = true;
                args.UserID = UserID;
                OnShowMultipleReviewQuizFeedback(args);
            }
            if ((SelectedCondition == "Number of Questions") && (NumberOfQuestion > int.Parse(ConditionValue)))
            {
                Message = "No more questions to review.";

                ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
                args.IsShown = true;
                args.UserID = UserID;
                OnShowMultipleReviewQuizFeedback(args);
            }
            else
            {
                QuestionModel question = Questions[QuestionNumber];
                CurrentQuestion = question;
                string questionData = JsonSerializer.Serialize<QuestionModel>(question);
                _serverService.SendDataToClients(questionData, "SendQuestion");
                TextAnswerInput = null;
                DisplayQuestion(question);
            }
        }

        
        public void ValidateClientAnswer(string? answer, int userID, string username, int totalPoints) 
        {
            string message = "";

            if (CurrentQuestion.Answer == answer)
            {
                NumberOfPointsGained = CurrentQuestion.NumberOfPoints;
                int numberOfPointsGained = totalPoints + CurrentQuestion.NumberOfPoints;
                message = $"Correct Answer. {CurrentQuestion.NumberOfPoints} points have been awarded!";
                _userService.UpdatePoints(userID, totalPoints);
                _multiplayerReviewQuizService.InsertMultiplayerQuizFeedbackData(SessionID, userID, QuizID, CurrentQuestion.Question, CurrentQuestion.Answer, true);
            }
            else
            {
                message = $"Answer was {CurrentQuestion.Answer}. 0 points have been awarded!";
                _multiplayerReviewQuizService.InsertMultiplayerQuizFeedbackData(SessionID, userID, QuizID, CurrentQuestion.Question, CurrentQuestion.Answer, true);
            }

            _serverService.SendDataToClients(message, "SendCorrectAnswer");
        }

        // pass event called end session?
        public void StopServer() 
        {
            _serverService.SendDataToClients("EndQuiz", "EndQuiz");
           _serverService.StopServer();

        }
    }
}
