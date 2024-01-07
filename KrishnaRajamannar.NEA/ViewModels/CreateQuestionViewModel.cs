using KrishnaRajamannar.NEA.Services;
using System.Collections.Generic;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class CreateQuestionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IQuestionService _questionService;
        private readonly IQuizService _quizService;
        private readonly IIndependentReviewQuizService _independentReviewQuizService;

        // Used to identify which quiz that the user is creating new questions for
        // This data is passed from the ViewQuizzes window through an event
        public int QuizID;

        public CreateQuestionViewModel(IQuestionService questionService, IQuizService quizService, IIndependentReviewQuizService independentReviewQuizService)
        {
            _questionService = questionService;
            _quizService = quizService;
            _independentReviewQuizService = independentReviewQuizService;
        }

        // Binds with the UI
        // Used to retrieve the user input for a question
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
        // Binds with the UI
        // Used to retrieve the input of the answer for a question
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
        // Option 1 to Option 6 are used to retrieve the inputs of the options
        // for a multiple choice based question 
        // They can be null as not all options need to be used

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
        // Binds with the UI
        // Used to retrieve the input of the time duration
        // which is how long users have to answer a question (in seconds)
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
        // Binds with the UI
        // Used to retrieve the input of the number of points 
        // which can be awarded when answering a question correctly
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
        // Binds with the UI
        // Used to display error messages when creating a question
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
        // Used to notify the UI of any changes in the values of properties
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        // Used to check if the user has entered a value for the question 
        public bool ValidateQuestion()
        {
            if (Question != null)
            {
                return true;
            }
            Message = "A question must be inputted.";
            return false;
        }
        public bool ValidateAnswer()
        {
            if (Answer != null)
            {
                Message = "An answer must be inputted.";
                return false;
            }
            return false;
        }
        // Used to ensure that option 1 and option 2 are not empty 
        // when creating a mutliple-choice based question
        public bool ValidateOptions() 
        { 
            if ((Option1 != null) || (Option2 != null))
            {
                return true;
            }
            Message = "Options 1 and 2 must have an answer";
            return false;
        }
        // Used to ensure that points inputs are between 1 and 5
        public bool ValidateNumberOfPoints()
        {
            if (NumberOfPoints != 0) 
            {
                if ((NumberOfPoints >= 1) && (NumberOfPoints <= 5))
                {
                    return true;
                }
                Message = "The number of points must be between 1 and 5.";
                return false;
            }
            Message = "The number of points cannot be 0.";
            return false;
        }
        // Used to ensure that duration input (how long users have to answer the question)
        // is within 10 seconds and 5 minutes
        public bool ValidateDuration() 
        {
            if (Duration != 0) 
            {
                if ((Duration >= 10) && (Duration <= 300))
                {
                    return true;
                }
                Message = "The time duration must be between 10 seconds and 300 seconds.";
                return false;
            }
            Message = "The time duration cannot be 0.";
            return false;
        }
        // Used to validate a text-based question
        public bool ValidateStandardInputs() 
        {
            if ((ValidateQuestion() != true) || (ValidateAnswer() != true) || (ValidateNumberOfPoints() != true) || (ValidateDuration() != true)) 
            {
                return false;
            }
            return true;
        }
        // Checks if the inputs are valid for a text-based question
        // Inserts the question data into the text question table in DB
        // Gets ID of question and then also inserts data into quiz feedback table
        public bool CreateTextQuestion() 
        {
            if (ValidateStandardInputs() == true)
            {
                _questionService.CreateTextQuestion(Question, Answer, Duration, NumberOfPoints, QuizID);

                _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(QuizID), QuizID);

                int textQuestionID = _questionService.GetTextQuestionID(Question, Answer, QuizID);

                _independentReviewQuizService.InsertTextQuestionQuizFeedback(textQuestionID, NumberOfPoints, QuizID);

                Message = "Successful Text Question Creation.";
                return true;
            }
            return false;
        }
        // Checks if the inputs and options are valid for a multiple-choice-based question
        // Inserts the question data into the multiple choice question table in DB
        // Gets ID of question and then also inserts data into quiz feedback table
        public bool CreateMultipleChoiceQuestion() 
        {
            if ((ValidateStandardInputs() == true) && (ValidateOptions() == true)) 
            {
                Dictionary<string, string> options = new Dictionary<string, string>();

                // Converts all 6 options into a Dictionary to prevent too many parameters for one procedure/function
                options = _questionService.GetOptions(Option1, Option2, Option3, Option4, Option5, Option6);

                _questionService.CreateMultipleChoiceQuestion(Question, Answer, Duration, NumberOfPoints, QuizID, options);

                _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(QuizID), QuizID);

                int MCQuestionID = _questionService.GetMultipleChoiceQuestionID(Question, Answer, QuizID);

                _independentReviewQuizService.InsertMultipleChoiceQuestionQuizFeedback(MCQuestionID, NumberOfPoints, QuizID);

                Message = "Successful Multiple Choice Question Creation.";
                return true;    
            }
            return false;
        }
    }
}
