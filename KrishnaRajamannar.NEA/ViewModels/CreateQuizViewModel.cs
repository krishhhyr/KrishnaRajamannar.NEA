using KrishnaRajamannar.NEA.Services;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class CreateQuizViewModel : INotifyPropertyChanged
    {

        // This instantiates the class which handles the data being sent and recieved from the database for the Quiz table.
        //public QuizService _quizService = new QuizService();
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IQuizService _quizService;
        // Used to identify which user is creating a quiz
        // Data is passed on from the ViewQuizzes window
        public int UserID;


        public CreateQuizViewModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // This code is used for data binding with the UI. 
        // Retrieves what the user inputs as the quiz title
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

        // Binds with the UI
        // Used to notify users of any errors with the quiz title input
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

        // Used to bind with the UI
        // Used to notify when the property's value changes
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }


        // This procedure takes in the userID as a parameter. 
        // When creating a quiz, if no title has been inputted, an error message is displayed.
        // Otherwise, the method in QuizService is called which takes in the userID and QuizTitle to insert the quiz in the database. 
        public bool CreateQuiz() 
        {
            // Checks if quiz title inout is not empty 
            if ((QuizTitle != null) || (QuizTitle == ""))
            {
                // Calls the service to create and store the quiz in the DB
                _quizService.CreateQuiz(UserID, QuizTitle);
                Message = "Quiz created.";
                return true;
            }
            else  
            {
               Message = "Invalid Input. Try again.";
                return false;
            }
        }
    }
}
