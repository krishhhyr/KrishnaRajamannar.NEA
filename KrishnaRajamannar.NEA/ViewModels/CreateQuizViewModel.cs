using KrishnaRajamannar.NEA.Services;
using Microsoft.Identity.Client;
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

        // This instantiates the class which handles the data being sent and recieved from the database for the Quiz table.
        public QuizService _quizService = new QuizService();

        public int UserID;


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

        // This code is used for data binding with the UI. 

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

        // This function takes in the userID as a parameter. 
        // When creating a quiz, if no title has been inputted, an error message is displayed.
        // Otherwise, the method in QuizService is called which takes in the userID and QuizTitle to insert the quiz in the database. 
        public void CreateQuiz(int? userID) 
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
