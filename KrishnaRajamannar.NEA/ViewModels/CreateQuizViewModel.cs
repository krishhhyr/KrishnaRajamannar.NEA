using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public void CreateQuiz(int userID) 
        {
            if (QuizTitle != null)
            {
                _quizService.CreateQuiz(userID, QuizTitle);
                MessageBox.Show("Quiz created.");
            }
            else  
            {
                MessageBox.Show("Invalid Input. Try again.");
            }
        }
    }
}
