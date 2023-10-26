using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Views;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class QuizQuestionViewModel
    {
        public QuizService _quizService { get; set; }
        public QuestionService _questionService { get; set; }

        //services
        private readonly IQuizService _quizServicea;
        private readonly IQuestionService _questionServicea;   
        //models 
        private readonly QuizModel quizModel;
        //windows 
        private readonly CreateQuestion createQuestion;
        private readonly CreateQuiz createQuiz; 


        //public QuizTitle SelectedRow { get; set; }

        public QuizQuestionViewModel()
        {
            _quizService = new QuizService();
            _questionService = new QuestionService();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

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
        public int GetRowQuizID() 
        {
            if (SelectedQuiz == null) 
            {
                return 0;
            }
            return SelectedQuiz.QuizID;
        }
        public IList<QuizModel> LoadQuiz(int? userID)
        {
            return _quizService.GetQuiz(userID);
        }

        // check if null
        public IList<QuizModel> DeleteQuiz(int? _userID)
        {
            if (SelectedQuiz == null)
            {
                return LoadQuiz(_userID);
            }
                
            int quizID = SelectedQuiz.QuizID;
            string quizTitle = SelectedQuiz.QuizTitle;
            int userID = SelectedQuiz.UserID;

            _quizService.DeleteQuiz(quizID, quizTitle, userID);

            return LoadQuiz(userID);
        }

        public IList<QuestionModel> LoadQuestions()
        {
            if (SelectedQuiz == null)
            {
                return null;
            }
            int quizID = SelectedQuiz.QuizID;

            return _questionService.GetQuestions(quizID);
        }

        public IList<QuestionModel> DeleteQuestions()
        {
            if (SelectedQuestion == null)
            {
                return null;
            }
            List<string?> options = new List<string?>();

            options.Add(SelectedQuestion.Option1);
            options.Add(SelectedQuestion.Option2);
            options.Add(SelectedQuestion.Option3);
            options.Add(SelectedQuestion.Option4);
            options.Add(SelectedQuestion.Option5);
            options.Add(SelectedQuestion.Option6);

            // https://www.techiedelight.com/check-if-all-items-are-same-in-a-list-in-csharp/
            bool isOptionsNull = options.Distinct().Count() == 1 ;

            int quizID = SelectedQuiz.QuizID;

            if (isOptionsNull == false) 
            {
                _questionService.DeleteMultipleChoiceQuestion(SelectedQuestion.Question, SelectedQuestion.Answer, SelectedQuiz.QuizID);

                _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);
            }
            if (isOptionsNull == true) 
            {
                _questionService.DeleteTextQuestion(SelectedQuestion.Question, SelectedQuestion.Answer, SelectedQuiz.QuizID);

                _quizService.UpdateNumberOfQuestions(_questionService.GetNumberOfQuestions(quizID), quizID);
            }

            return _questionService.GetQuestions(quizID);
        }
    }
}
