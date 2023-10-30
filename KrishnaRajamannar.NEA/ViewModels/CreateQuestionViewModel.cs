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
    public class CreateQuestionViewModel : INotifyPropertyChanged
    {
        public QuestionService _questionService { get; set; }

        public QuizService _quizService { get; set; }

        public IndependentReviewQuizService _independentReviewQuizService { get; set;}

        public int QuizID;

        public CreateQuestionViewModel()
        {
            _questionService = new QuestionService();
            _quizService = new QuizService();
            _independentReviewQuizService = new IndependentReviewQuizService();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
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
        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                RaisePropertyChange("Answer");
            }
        }
        private string? _option1;
        public string? Option1
        {
            get { return _option1; }
            set
            {
                _option1 = value;
                RaisePropertyChange("Option1");
            }
        }
        private string? _option2;
        public string? Option2
        {
            get { return _option2; }
            set
            {
                _option2 = value;
                RaisePropertyChange("Option2");
            }
        }
        private string? _option3;
        public string? Option3
        {
            get { return _option3; }
            set
            {
                _option3 = value;
                RaisePropertyChange("Option3");
            }
        }
        private string? _option4;
        public string? Option4
        {
            get { return _option4; }
            set
            {
                _option4 = value;
                RaisePropertyChange("Option4");
            }
        }
        private string? _option5;
        public string? Option5
        {
            get { return _option5; }
            set
            {
                _option5 = value;
                RaisePropertyChange("Option5");
            }
        }
        private string? _option6;
        public string? Option6
        {
            get { return _option6; }
            set
            {
                _option6 = value;
                RaisePropertyChange("Option6");
            }
        }
        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                RaisePropertyChange("Duration");
            }
        }
        private int _numberOfPoints;
        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set
            {
                _numberOfPoints = value;
                RaisePropertyChange("NumberOfPoints");
            }
        }

        public bool ValidateQuestion()
        {
            if (Question == null)
            {
                return false;
            }
            return true;
        }
        public bool ValidateAnswer()
        {
            if (Answer == null)
            {
                return false;
            }
            return true;
        }
        public bool ValidateOptions() 
        {
            if ((Option1 == null) || (Option2 == null)) 
            {
                return false;
            }
            return true;
        }
        public bool ValidateNumberOfPoints()
        {
            if ((NumberOfPoints >= 1) && (NumberOfPoints <= 5))
            {
                return true;
            }
            return false;
        }
        public bool ValidateDuration() 
        {
            if (Duration == 0) 
            {
                return false;
            }
            return true;
        }

        public void CreateTextQuestion(int quizID) 
        {
            if ((ValidateQuestion() == false) || (ValidateAnswer() == false)) 
            {
                MessageBox.Show("A question or an answer has not been inputted. Try again.", "Question Creation");
            } 
            if ((ValidateNumberOfPoints() == false) || (ValidateDuration() == false))
            {
                MessageBox.Show("The number of points must be between 1-5. The duration must not be 0. Try again.", "Question Creation");
            }
            _questionService.CreateTextQuestion(Question, Answer, Duration, NumberOfPoints, quizID);

            _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);

            int textQuestionID = _questionService.GetTextQuestionID(Question, Answer, quizID);

            _independentReviewQuizService.InsertTextQuestionQuizFeedback(textQuestionID, NumberOfPoints, quizID);

            MessageBox.Show("Successful Text Question Creation.", "Question Creation");
        }
        public void CreateMultipleChoiceQuestion(int quizID) 
        {
            if ((ValidateQuestion() == false) || (ValidateAnswer() == false))
            {
                MessageBox.Show("A question or an answer has not been inputted. Try again.", "Question Creation");
            }
            if (ValidateOptions() == false) 
            {
                MessageBox.Show("Option 1 and Option 2 must not be empty. Try again.", "Question Creation");
            }
            if ((ValidateNumberOfPoints() == false) || (ValidateDuration() == false))
            {
                MessageBox.Show("The number of points must be between 1-5. The duration must not be 0. Try again.", "Question Creation");
            }
            
            Dictionary<string, string> options = new Dictionary<string, string>();

            options = _questionService.GetOptions(Option1, Option2, Option3, Option4, Option5, Option6);

            _questionService.CreateMultipleChoiceQuestion(Question, Answer, Duration, NumberOfPoints, quizID, options);

            _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);

            int MCQuestionID = _questionService.GetMultipleChoiceQuestionID(Question, Answer, quizID);

            _independentReviewQuizService.InsertMultipleChoiceQuestionQuizFeedback(MCQuestionID, NumberOfPoints, quizID);

            MessageBox.Show("Successful Multiple Choice Question Creation.", "Question Creation");
        }
    }
}
