﻿using KrishnaRajamannar.NEA.Models.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ViewSessionInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
      

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
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

        public void LoadData(ServerResponse response) 
        {
            if(response != null)
            {
                _sessionId = response.SessionId;
                if(!string.IsNullOrEmpty(response.Data))
                {
                    SessionData data = JsonSerializer.Deserialize<SessionData>(response.Data);
                    if (data != null)
                    {
                        _quizSelected = data.QuizSelected;
                        _hostName= data.HostName;
                        if(data.UserSessions.Any())
                        {
                            _userSessionData.Clear();
                            _userSessionData.AddRange(data.UserSessions);
                        }
                    }                    
                }
            }
        }
    }
}