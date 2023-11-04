using Azure;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class HostSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ISessionService _sessionService;

        private readonly IQuizService _quizService;

        public int UserID;
        IList<QuizModel> quizzes = new List<QuizModel>();

        public HostSessionViewModel(ISessionService sessionService, IQuizService quizService)
        {
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
                _endQuizConditionInput = value;
                RaisePropertyChange("ConditionInput");
            }
        }
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        public int CreateSessionID() 
        {
            Random random = new Random();
            int sessionID = 0;
            bool valid = false;
            while (valid == false) 
            {
                sessionID = random.Next(100000, 10000000);
                if (_sessionService.IsSessionIDExist(sessionID) == false) 
                {
                    valid = true;
                }
            }
            return sessionID;
        }
        public IPAddress? GetIPAddress() 
        {
            return null;
        }
        public int? GetPortNumber() 
        {
            Random random = new Random();
            int portNumber = 0;
            bool valid = false;
            while (valid == false)
            {
                portNumber = random.Next(49152, 65536);
                if (_sessionService.IsPortNumberExist(portNumber) == false)
                {
                    valid = true;
                }
            }
            return sessionID;
        }

        public void GetQuizzes()
        {
            List<string> titlesOfQuizzes = new List<string>();

            quizzes = _quizService.GetQuiz(UserID);

            foreach (var quiz in quizzes)
            {
                titlesOfQuizzes.Add(quiz.QuizTitle);
            }
            _quizTitles = titlesOfQuizzes;
        }
        public IList<QuizModel>? GetQuestions(int quizID) 
        {
            return null;
        }
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
                    //Show a message...This quiz only has 4 questions, input a lower number of questions
                    return false;
                }
            }
            else 
            {
                //show a message saying that the condition input was not in the correct format
                return false;
            }
            return false;
        }
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
                //Show a message...
                return false;
            }
            return false;
        }
    } 
}
