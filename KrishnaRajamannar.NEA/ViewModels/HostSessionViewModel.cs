using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class HostSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IQuizService _quizService;

        public int UserID;

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
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public void GetQuizzes() 
        {
            IList<QuizModel> quizzes = new List<QuizModel>();
            quizzes = _quizService.GetQuiz(UserID);

            foreach (var quiz in quizzes) 
            {
                QuizTitles.Add(quiz.QuizTitle);
            }
        }


    }
}
