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

        private readonly IQuizService _quizService;

        public int UserID;
        IList<QuizModel> quizzes = new List<QuizModel>();

        public HostSessionViewModel(IQuizService quizService)
        {
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
        private string _endQuizConditionInput;
        public string EndQuizConditionInput
        {
            get { return _endQuizConditionInput; }
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
        public IPAddress? GetIPAddress() 
        {
            return null;
        }
        public int? GetPortNumber() 
        {
            return null;
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
        public void ValidateNumberOfQuestionsInput(string quizTitle) 
        {
            bool valid = false;

            foreach (var quiz in quizzes) 
            {
                if ((quizTitle == quiz.QuizTitle) && (int.Parse(EndQuizConditionInput) <= quiz.NumberOfQuestions)) 
                {
                    GetQuestions(quiz.QuizID);
                    valid = true;
                }
            }
            if (valid == false) 
            {
                //Show a message...
            }
        }
        public void ValidateTimeInput(string quizTitle)
        {
            if ((int.Parse(EndQuizConditionInput) >= 5) && (int.Parse(EndQuizConditionInput) <= 60))
            {
                foreach (var quiz in quizzes)
                {
                    if (quizTitle == quiz.QuizTitle)
                    {
                        GetQuestions(quiz.QuizID);
                    }
                }
            }
            else 
            {
                //Show a message...
            }
        }
    } 
}
