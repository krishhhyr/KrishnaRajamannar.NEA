using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ClientSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IClientService _clientService;
        public ClientSessionViewModel(IClientService clientService)
        {
            _clientService = clientService;
            _clientService.ClientConnected += OnClientConnected;
            _clientService.StartQuizEvent += OnStartQuizEvent;
            _clientService.ProcessCommand += OnProcessCommand;
        }

        private void OnProcessCommand(object sender, Events.ProcessCommandEventArgs e)
        {
            ProcessCommand(e.ServerResponse);
        }

        private void OnStartQuizEvent(object sender, Events.StartQuizEventArgs e)
        {
            ProcessCommand(e.ServerResponse);
        }

        private void OnClientConnected(object sender, Events.ClientConnectedEventArgs e)
        {
            LoadData(e.ServerResponse);
        }

        public void ConnectToServer()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate ()
            {
                _clientService.ConnectToServer("Krishna001", 1, "192.168.0.65", 59763, "107450");
            });            
        }       

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

        public void ProcessCommand(ServerResponse response)
        {
            if (response != null)
            {
                SessionId = response.SessionId;
                DataType = response.DataType;
                if (!string.IsNullOrEmpty(response.Data))
                {
                    QuizModel quizData = JsonSerializer.Deserialize<QuizModel>(response.Data);
                }
            }
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
                        _quizSelected = data.QuizSelected;
                        _hostName = data.HostName;
                        _endQuizConditionSelected = data.EndQuizCondition;
                        _endQuizConditionValue = data.EndQuizConditionValue;

                        if (data.UserSessions.Any())
                        {
                            _userSessionData.Clear();
                            _userSessionData.AddRange(data.UserSessions);

                            _numberofJoinedUsers = data.UserSessions.Count;
                        }
                    }
                }
            }
        }
    }
}
