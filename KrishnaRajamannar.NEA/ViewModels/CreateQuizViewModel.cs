using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class CreateQuizViewModel:INotifyPropertyChanged
    {
        public QuizService _quizService = new QuizService();

        public CreateQuizViewModel()
        {
            _quizService = new QuizService();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _quizTitle;
        public string QuizTitle 
        {
            get { return _quizTitle; }
            set 
            {
                _quizTitle = value;
                RaisePropertyChange("QuizTitle");
            }
        }

        public bool CreateQuiz(int userID) 
        {
            if (QuizTitle == null)
            {
                return false;
            }
            if (QuizTitle != null) 
            {
                _quizService.CreateQuiz(userID, QuizTitle);
                return true;
            }
            return false;
        }
    }
}
