using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class ViewQuizzesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Set of events used to handle showing/hiding different windows 
        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewQuizWindow;
        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewFeedbackWindow;
        public event ShowQuizParameterWindowEventHandler ShowCreateQuestionWindow;
        public event ShowAccountParameterWindowEventHandler ShowCreateQuizWindow;
        public event HideWindowEventHandler HideViewQuizzesWindow;

        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;

        public CreateQuizViewModel CreateQuizViewModel;
        public CreateQuestionViewModel CreateQuestionViewModel;
        public IndependentReviewViewModel IndependentReviewViewModel;   
        public IndependentReviewQuizFeedbackViewModel IndependentReviewFeedbackViewModel;

        // Variables which were used to retrieve data from the MainMenuViewModel
        public int UserID;
        public int QuizID;

        public ViewQuizzesViewModel(IQuizService quizService, IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;

            CreateQuizViewModel = App.ServiceProvider.GetService(typeof(CreateQuizViewModel)) as CreateQuizViewModel;
            CreateQuestionViewModel = App.ServiceProvider.GetService(typeof(CreateQuestionViewModel)) as CreateQuestionViewModel;
            IndependentReviewViewModel = App.ServiceProvider.GetService(typeof(IndependentReviewViewModel)) as IndependentReviewViewModel;
            IndependentReviewFeedbackViewModel = App.ServiceProvider.GetService(typeof(IndependentReviewQuizFeedbackViewModel)) as IndependentReviewQuizFeedbackViewModel;
        }

        // Used to identify which quiz that the user has selected 
        // Used to load the questions of that quiz and review the quiz
        private QuizModel _selectedQuiz;
        public QuizModel SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                _selectedQuiz = value;
                RaisePropertyChange("SelectedQuiz");
            }
        }
        // Used to identify which question that the user has selected within a quiz
        // Used to delete the selected question from the quiz 
        private QuestionModel _selectedQuestion;
        public QuestionModel SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                RaisePropertyChange("SelectedQuestion");
            }
        }

        // Used to display messages to users about the status of processing 
        // for the quizzes and question
        // Used to notify users about whether a quiz/question has been deleted
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

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        // HideViewQuizzes() and OnHideViewQuizzesWindow are part of an event used to 
        // hide the ViewQuizzes window once a new window has been displayed
        public void HideViewQuizzes()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideViewQuizzesWindow(args);
        }
        // ShowIndependentReviewQuiz and OnShowIndependentReviewWindow are part of an event used to 
        // show the IndependentReviewQuiz window which pass the quizID and the userID to 
        // identify which quiz that the user would like to review
        public void ShowIndependentReviewQuiz()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            args.QuizID = SelectedQuiz.QuizID;
            args.UserID = UserID;
            OnShowIndependentReviewQuizWindow(args);
        }
        // ShowIndependentReviewFeedback and OnShowIndependentReviewFeedbackWindow are part of an event used to 
        // show the IndependentReviewFeedback window which pass the quizID of the quiz selected by the user
        // which is used to identify which quiz feedback to display
        public void ShowIndependentReviewFeedback()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            args.QuizID = SelectedQuiz.QuizID;
            OnShowIndependentReviewFeedbackWindow(args);
        }
        // ShowCreateQuiz and OnShowCreateQuizWindow are part of an event used to 
        // show the CreateQuiz window which pass the user ID of the quiz selected by the user
        // which is used to identify which user is creating the new quiz
        public void ShowCreateQuiz()
        {
            ShowAccountParameterWindowEventArgs args = new ShowAccountParameterWindowEventArgs();
            args.IsShown = true;
            args.UserID = UserID;
            OnShowCreateQuizWindow(args);
        }
        // ShowCreateQuestion and OnShowCreateQuestopmWindow are part of an event used to 
        // show the CreateQuestion window which pass the quiz ID of the quiz selected by the user
        // which is used to identify which quiz will store the new question
        public void ShowCreateQuestion()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            args.QuizID = SelectedQuiz.QuizID;
            OnShowCreateQuestionWindow(args);
        }
        protected virtual void OnHideViewQuizzesWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideViewQuizzesWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowIndependentReviewQuizWindow(ShowQuizParameterWindowEventArgs e)
        {
            ShowQuizParameterWindowEventHandler handler = ShowIndependentReviewQuizWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowIndependentReviewFeedbackWindow(ShowQuizParameterWindowEventArgs e)
        {
            ShowQuizParameterWindowEventHandler handler = ShowIndependentReviewFeedbackWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowCreateQuizWindow(ShowAccountParameterWindowEventArgs e)
        {
            ShowAccountParameterWindowEventHandler handler = ShowCreateQuizWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnShowCreateQuestionWindow(ShowQuizParameterWindowEventArgs e)
        {
            ShowQuizParameterWindowEventHandler handler = ShowCreateQuestionWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Used to retrieve all the quizzes from the DB for the user 
        // and store them as a list of objects
        public IList<QuizModel> LoadQuiz()
        {
            Message = "";
            IList<QuizModel> quiz = _quizService.GetQuiz(UserID);
            if (quiz.Count != 0)
            {
                return quiz;
            }
            else 
            {
                Message = "No quizzes have been created. Right click to create a new quiz.";
                return null;
            }
        }
        public IList<QuizModel> DeleteQuiz()
        {
            // Checks if the user has selected a quiz to delete
            if (SelectedQuiz != null)
            {
                // Calls the quizService to delete the selected quiz
                _quizService.DeleteQuiz(SelectedQuiz.QuizID, SelectedQuiz.QuizTitle, SelectedQuiz.UserID);
                // Displays a message to users to notify them that the quiz has been deleted
                Message = "Quiz Deleted.";
                // Reloads the list of quizzes after deleting a quiz
                return LoadQuiz();
            }
            else 
            {
                Message = "No quiz has been selected to delete.";
                return LoadQuiz();
            }
        }
        // Used to load the questions in the data grid for a selected quiz
        public IList<QuestionModel> LoadQuestions()
        {
            // Checks if the user has selected a quiz to load the questions for
            if (SelectedQuiz != null)
            {
                Message = "";
                // Used to return an IList of all the questions for the selected quiz
                IList<QuestionModel> questions = _questionService.GetQuestions(SelectedQuiz.QuizID);
                if (questions.Count != 0)
                {
                    return questions;
                }
                else 
                {
                    Message = "There are no questions within this quiz.";
                    return null;
                }
            }
            else 
            {
                Message = "A quiz has not been selected.";
                return null;
            }
        }
        public IList<QuestionModel> DeleteQuestions()
        {
            // Checks if the user has actually selected a question
            if (SelectedQuestion != null)
            {
                // Creates a list of options for the selected question
                List<string?> options = new List<string?>();

                options.Add(SelectedQuestion.Option1);
                options.Add(SelectedQuestion.Option2);
                options.Add(SelectedQuestion.Option3);
                options.Add(SelectedQuestion.Option4);
                options.Add(SelectedQuestion.Option5);
                options.Add(SelectedQuestion.Option6);

                // Checks if all the options have the same value (i.e NULL)
                bool isOptionsNull = options.Distinct().Count() == 1;

                int quizID = SelectedQuiz.QuizID;

                // if all the options have the same value, it means that the quesion must be a text-based question
                // Otherwise, it's a multiple-choice based question
                if (isOptionsNull != true)
                {
                    // Deletes the question from the multiple choice table
                    _questionService.DeleteMultipleChoiceQuestion(SelectedQuestion.Question, SelectedQuestion.Answer, SelectedQuiz.QuizID);

                    // Updates the number of questions in the table
                    _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);
                }
                else 
                {
                    // Deletes the question from the text question table
                    _questionService.DeleteTextQuestion(SelectedQuestion.Question, SelectedQuestion.Answer, SelectedQuiz.QuizID);

                    _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);
                }
                // Notifies users that the question has been deleted
                Message = "Question Deleted.";
                // Reloads the questions in the quiz selected
                return LoadQuestions();
            }
            else 
            {
                return null;
            }
            
        }
    }
}
