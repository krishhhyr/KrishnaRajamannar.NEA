using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.ViewModels
{
    // Inherits the NotifyPropertyChanged interface 
    public class ClientSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event QuestionRecievedEventHandler TextQuestionRecieved;
        public event QuestionRecievedEventHandler MultipleChoiceQuestionRecieved;
        public event ShowAccountParameterWindowEventHandler ShowMultipleQuizFeedbackWindow;
        public event TimerEventHandler AnswerTimerFinished;
        private readonly IClientService _clientService;
        private readonly ISessionService _sessionService;

        public MultipleReviewQuizFeedbackViewModel MultipleReviewQuizFeedbackViewModel;

        // These are variables which have data being passed from the Main Menu window
        // This represents the user data of the client connecting to a session
        public string Username;
        public int UserID;
        public int TotalPoints;
        // This is used to keep track of the question that the quiz is currently on
        public int QuestionNumber = 1;

        private DispatcherTimer answerTimer;
        private TimeSpan AnswerTime;

        public ClientSessionViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;

            //There can be only one instance worker thread that process client service
            _clientService = new ClientService();
            answerTimer = new DispatcherTimer();

            MultipleReviewQuizFeedbackViewModel = App.ServiceProvider.GetService(typeof(MultipleReviewQuizFeedbackViewModel)) as MultipleReviewQuizFeedbackViewModel;

            // These represent events which have been subscribed to
            answerTimer.Tick += AnswerTimer_Tick;
            _clientService.ClientConnected += OnClientConnected;
            _clientService.StartQuizEvent += OnStartQuizEvent;
            _clientService.ProcessServerResponse += OnProcessServerResponse;
            _clientService.EndQuizEvent += OnEndQuizEvent;

        }

        // This is used to display the quiz feedback once the client side receives a response from the server/host about 
        // ending the quiz review 
        private void OnEndQuizEvent(object sender, EndQuizEventArgs e)
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = UserID;
            OnShowMultipleQuizFeedbackWindow(args);
        }

        // Passes the server responses that have been received
        private void OnProcessServerResponse(object sender, Events.ProcessServerResponseEventArgs e)
        {
            ProcessServerResponse(e.ServerResponse);
        }

        private void OnStartQuizEvent(object sender, Events.StartQuizEventArgs e)
        {
            ProcessServerResponse(e.ServerResponse);
        }

        // Used to load the session data to the UI after the server has responded after a client connects to a session
        private void OnClientConnected(object sender, Events.ClientConnectedEventArgs e)
        {
            LoadData(e.ServerResponse);
        }

        #region Properties

        // Used to bind with UI
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _hostName;
        public string HostName
        {
            get { return _hostName; }
            set
            {
                _hostName = value;
                RaisePropertyChange("HostName");
            }
        }

        private string _sessionId;
        public string SessionId
        {
            get { return _sessionId; }
            set
            {
                _sessionId = value;
                RaisePropertyChange("SessionId");
            }
        }

        private string _quizSelected;
        public string QuizSelected
        {
            get { return _quizSelected; }
            set
            {
                _quizSelected = value;
                RaisePropertyChange("QuizSelected");
            }
        }

        private string _endQuizConditionSelected;
        public string EndQuizConditionSelected
        {
            get { return _endQuizConditionSelected; }
            set
            {
                _endQuizConditionSelected = value;
                RaisePropertyChange("EndQuizConditionSelected");
            }
        }

        private string _endQuizConditionValue;
        public string EndQuizConditionValue
        {
            get { return _endQuizConditionValue; }
            set
            {
                _endQuizConditionValue = value;
                RaisePropertyChange("EndQuizConditionValue");
            }
        }

        private string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                RaisePropertyChange("DataType");
            }
        }

        // Used to bind with a data grid that displays the user details of all the users
        // who have joined a session
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

        private int _numberofJoinedUsers;
        public int NumberofJoinedUsers
        {
            get { return _numberofJoinedUsers; }
            set
            {
                _numberofJoinedUsers = value;
                RaisePropertyChange("NumberofJoinedUsers");
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

        private string _answerInput;
        public string AnswerInput
        {
            get { return _answerInput; }
            set
            {
                _answerInput = value;
                RaisePropertyChange("AnswerInput");
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

        private string _remainingTimeLimit;
        public string RemainingTimeLimit
        {
            get { return _remainingTimeLimit; }
            set
            {
                _remainingTimeLimit = value;
                RaisePropertyChange("RemainingTimeLimit");
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

        #endregion

        // An event which is used to change the values of UI elements when 
        // a text-based question is displayed
        protected virtual void OnShowTextQuestion(QuestionRecievedEventArgs e) 
        {
            QuestionRecievedEventHandler handler = TextQuestionRecieved;
            if (handler != null) 
            {
                handler(this, e);
            }
        }

        // An event which is used to change the values of UI elements when 
        // a multiple-choice question is displayed
        protected virtual void OnShowMultipleChoiceQuestion(QuestionRecievedEventArgs e) 
        {
            QuestionRecievedEventHandler handler = MultipleChoiceQuestionRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Used to disable UI elements when the answer timer has reached 0
        protected virtual void OnDisableAnsweringQuestion(TimerEventArgs e) 
        {
            TimerEventHandler handler = AnswerTimerFinished;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
        // Used to display the MultipleQuizFeedbackWindow
        protected virtual void OnShowMultipleQuizFeedbackWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowMultipleQuizFeedbackWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Used to process all the general server responses that a client recieves 
        public void ProcessServerResponse(ServerResponse response)
        {
            if (response != null)
            {
                SessionId = response.SessionId;
                DataType = response.DataType;
                if (!string.IsNullOrEmpty(response.Data))
                {
                    switch (response.DataType) 
                    {
                        // For this case, this is used when the server sends a question to the client to review
                        case "SendQuestion":
                            QuestionModel question = JsonSerializer.Deserialize<QuestionModel>(response.Data);
                            AnswerInput = null;

                            // Checks the type of question 
                            // If question.Option = "", then it is a text-based question
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
                            NumberOfQuestion = QuestionNumber;
                            QuestionNumber++;
                            // Used to assign the values of the question to UI elements
                            AssignQuestionValues(question);
                            AssignAnswerTimeValues(question.Duration);
                            answerTimer.Start();
                            break;
                            // For this case, this is used when the server sends the correct answer for a question back to the client 
                        case "SendCorrectAnswer":
                            ValidAnswerMessage = response.Data;
                            RetrieveNumberOfPoints(ValidAnswerMessage);

                            if (NumberOfPointsGained > 0)
                            {
                                Message = "Correct Answer!";
                            }
                            else 
                            {
                                Message = "Incorrect Answer!";
                            }
                            break;
                    }
                }
            }
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

        // Used to assign the value of the answer timer
        private void AssignAnswerTimeValues(int answerTime) 
        {
            AnswerTimeLimit = answerTime.ToString();
            AnswerTime = TimeSpan.FromSeconds(answerTime);
            answerTimer.Interval = TimeSpan.FromSeconds(1);
        }

        // Used to decrement the answer timer
        private void AnswerTimer_Tick(object? sender, EventArgs e)
        {
            // When the answerTime reaches 0, the answer inputted is sent back to the server
            if (AnswerTime == TimeSpan.Zero) 
            {
                answerTimer.Stop();
                SendAnswer();
            }
            else 
            {
                AnswerTime = AnswerTime.Add(TimeSpan.FromSeconds(-1));
                RemainingTimeLimit = AnswerTime.Seconds.ToString();
            }
        }

        // This is used to connect to the server in order to join a session
        public void ConnectToServer()
        {
            // Used to retrieve the IP address and port number of the host's machine to connect to
            // Uses a Tuple structure to store the both values in one place
            (string, int) connectionInfo = _sessionService.GetConnectionData(Convert.ToInt32(SessionId));

            string ipAddressConnect = connectionInfo.Item1;
            int portNumberConnect = connectionInfo.Item2;

            _clientService.ConnectToServer(Username, UserID, ipAddressConnect, portNumberConnect, SessionId.ToString());

            Message = "Connected.";
        }

        // Used to send the answer inputted to the server once the time reaches 0
        public void SendAnswer() 
        {
            TimerEventArgs args = new TimerEventArgs();
            OnDisableAnsweringQuestion(args);
            Message = "Times up! Validating Response...";
            // Sends the answer to the server
            _clientService.SendDataToServer(AnswerInput, "SendAnswer", UserID, Username, TotalPoints);
        }

        // Used to break down the message that the server sent back 
        // Used to retrieve the number of points that have been gained for a question
        public void RetrieveNumberOfPoints(string validAnswerMessage) 
        {
            // Splits the two sentences "Correct Answer. 3 points have been awarded!"
            // into "3 points been awarded!"
            string[] spiltMessage = validAnswerMessage.Split('.');
            // Splits the sentence into an array of words 
            // {"", "3", "points", "have","been","awarded!"}
            string[] test = spiltMessage[1].Split(' ');
            NumberOfPointsGained = NumberOfPointsGained + Convert.ToInt32(test[1]);
        }

        // Used to load the data grid with the details of the users who join a session
        // and it is used to notify the UI of the details of the session which has 
        // been joined
        public void LoadData(ServerResponse response)
        {
            if (response != null)
            {
                SessionId = response.SessionId;
                DataType = response.DataType;
                if (!string.IsNullOrEmpty(response.Data))
                {
                    SessionData data = JsonSerializer.Deserialize<SessionData>(response.Data);
                    // This data represents the details of the session which has been joined
                    if (data != null)
                    {
                        QuizSelected = data.QuizSelected;
                        HostName = data.HostName;
                        EndQuizConditionSelected = data.EndQuizCondition;
                        EndQuizConditionValue = data.EndQuizConditionValue;

                        if (data.UserSessions.Any())
                        {
                            JoinedUsers.Clear();
                            JoinedUsers = data.UserSessions.ToList();

                            NumberofJoinedUsers = JoinedUsers.Count;
                        }
                    }
                }
            }
        }
    }
}
