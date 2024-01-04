using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ClientSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event QuestionRecievedEventHandler TextQuestionRecieved;
        public event QuestionRecievedEventHandler MultipleChoiceQuestionRecieved;
        public event ShowSessionParameterWindowEventHandler ShowMultipleQuizFeedbackWindow;
        public event TimerEventHandler AnswerTimerFinished;
        private readonly IClientService _clientService;
        private readonly ISessionService _sessionService;

        public MultipleReviewQuizFeedbackViewModel MultipleReviewQuizFeedbackViewModel;

        public string Username;
        public int UserID;
        public int TotalPoints;
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

            answerTimer.Tick += AnswerTimer_Tick;
            _clientService.ClientConnected += OnClientConnected;
            _clientService.StartQuizEvent += OnStartQuizEvent;
            _clientService.ProcessServerResponse += OnProcessServerResponse;
            _clientService.EndQuizEvent += OnEndQuizEvent;

        }

        private void OnEndQuizEvent(object sender, EndQuizEventArgs e)
        {
            ShowSessionParameterWindowEventArgs args = new ShowSessionParameterWindowEventArgs();
            args.ServerResponse = e.ServerResponse;
            OnShowMultipleQuizFeedbackWindow(args);
        }

        private void OnProcessServerResponse(object sender, Events.ProcessServerResponseEventArgs e)
        {
            ProcessServerResponse(e.ServerResponse);
        }

        private void OnStartQuizEvent(object sender, Events.StartQuizEventArgs e)
        {
            ProcessServerResponse(e.ServerResponse);
        }

        private void OnClientConnected(object sender, Events.ClientConnectedEventArgs e)
        {
            LoadData(e.ServerResponse);
        }

        #region Properties

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

        private List<UserSessionData> _userSessionData = new List<UserSessionData>();
        public List<UserSessionData> UserSessionData
        {
            get { return _userSessionData; }
            set
            {
                _userSessionData = value;
                RaisePropertyChange("UserSessionData");
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

        protected virtual void OnShowMultipleQuizFeedbackWindow(ShowSessionParameterWindowEventArgs e)
        {
            ShowSessionParameterWindowEventHandler handler = ShowMultipleQuizFeedbackWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // need an event for each question type?
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
                        case "SendQuestion":
                            QuestionModel question = JsonSerializer.Deserialize<QuestionModel>(response.Data);
                            // mc question

                            AnswerInput = null;

                            if (question.Option1 != "NULL")
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
                            AssignQuestionValues(question);
                            AssignAnswerTimeValues(question.Duration);
                            answerTimer.Start();
                            break;
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

        // change NULL bit in Question service!
        private void AssignQuestionValues(QuestionModel questionData) 
        {
            Question = questionData.Question;
            AnswerTimeLimit = questionData.Duration.ToString();
            Option1 = questionData.Option1;
            Option2 = questionData.Option2;
            Option3 = questionData.Option3;
            Option4 = questionData.Option4;
            Option5 = questionData.Option5;
            Option6 = questionData.Option6;
        }

        private void AssignAnswerTimeValues(int answerTime) 
        {
            AnswerTimeLimit = answerTime.ToString();
            AnswerTime = TimeSpan.FromSeconds(answerTime);
            answerTimer.Interval = TimeSpan.FromSeconds(1);
        }

        // need to send event to stop answering when timer is up...
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
                RemainingTimeLimit = AnswerTime.Seconds.ToString();
            }
        }

        public void ConnectToServer()
        {
            (string, int) connectionInfo = _sessionService.GetConnectionData(Convert.ToInt32(SessionId));

            string ipAddressConnect = connectionInfo.Item1;
            int portNumberConnect = connectionInfo.Item2;

            _clientService.ConnectToServer(Username, UserID, ipAddressConnect, portNumberConnect, SessionId.ToString());

            Message = "Connected.";
        }

        public void SendAnswer() 
        {
            TimerEventArgs args = new TimerEventArgs();
            OnDisableAnsweringQuestion(args);
            Message = "Times up! Validating Response...";
            _clientService.SendDataToServer(AnswerInput, "SendAnswer", UserID, Username, TotalPoints);
        }

        public void RetrieveNumberOfPoints(string validAnswerMessage) 
        {
            string[] spiltMessage = validAnswerMessage.Split('.');
            string[] test = spiltMessage[1].Split(' ');
            NumberOfPointsGained = Convert.ToInt32(test[1]);
        }

        public void LoadData(ServerResponse response)
        {
            if (response != null)
            {
                SessionId = response.SessionId;
                DataType = response.DataType;
                if (!string.IsNullOrEmpty(response.Data))
                {
                    SessionData data = JsonSerializer.Deserialize<SessionData>(response.Data);
                    if (data != null)
                    {
                        QuizSelected = data.QuizSelected;
                        HostName = data.HostName;
                        EndQuizConditionSelected = data.EndQuizCondition;
                        EndQuizConditionValue = data.EndQuizConditionValue;

                        if (data.UserSessions.Any())
                        {
                            List<string> users = new List<string>();

                            UserSessionData.Clear();
                            
                            //_userSessionData.AddRange(data.UserSessions);
                            UserSessionData = data.UserSessions.ToList();
                            NumberofJoinedUsers = _userSessionData.Count;
                        }
                    }
                }
            }
        }
    }
}
