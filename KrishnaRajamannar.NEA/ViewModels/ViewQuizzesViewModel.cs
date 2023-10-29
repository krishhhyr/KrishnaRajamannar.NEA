using KrishnaRajamannar.NEA.Events;
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
    public class ViewQuizzesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event ShowMessageEventHandler ShowMessage;

        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewQuizWindow;

        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewFeedbackWindow;

        public event ShowQuizParameterWindowEventHandler ShowCreateQuestionWindow;

        public event ShowQuizParameterWindowEventHandler ShowCreateQuizWindow;

        public event HideWindowEventHandler HideViewQuizzesWindow;

        private readonly IQuizService _quizService;

        private readonly IQuestionService _questionService;

        private readonly QuizModel _quizModel;

        private readonly UserModel _questionModel;

        public int? UserID;
        public int QuizID;

        public ViewQuizzesViewModel(IQuizService quizService, IQuestionService questionService, QuizModel quizModel, UserModel questionModel)
        {
            _quizService = quizService;
            _questionService = questionService;
            _quizModel = quizModel;
            _questionModel = questionModel;

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

        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private void ShowMessageDialog(string message)
        {
            ShowMessageEventArgs args = new ShowMessageEventArgs();
            args.Message = message;
            OnShowMessage(args);
        }
        private void HideViewQuizzes()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideViewQuizzesWindow(args);
        }
        private void ShowIndependentReviewQuiz()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            OnShowIndependentReviewQuizWindow(args);
        }
        private void ShowIndependentReviewFeedback()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            OnShowIndependentReviewFeedbackWindow(args);
        }
        private void ShowCreateQuiz()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            OnShowCreateQuizWindow(args);
        }
        private void ShowCreateQuestion()
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            OnShowCreateQuestionWindow(args);
        }
        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;
            if (handler != null)
            {
                handler(this, e);
            }
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
        protected virtual void OnShowCreateQuizWindow(ShowQuizParameterWindowEventArgs e)
        {
            ShowQuizParameterWindowEventHandler handler = ShowCreateQuizWindow;
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

        public IList<QuizModel> LoadQuiz()
        {

            return _quizService.GetQuiz(UserID);
        }
        public IList<QuizModel> DeleteQuiz()
        {
            if (SelectedQuiz != null)
            {
                return LoadQuiz();
            }

            _quizService.DeleteQuiz(SelectedQuiz.QuizID, SelectedQuiz.QuizTitle, SelectedQuiz.UserID);

            return LoadQuiz();
        }
        public IList<QuestionModel> LoadQuestions()
        {
            if (SelectedQuiz == null)
            {
                return null;
            }

            return _questionService.GetQuestions(SelectedQuiz.QuizID);
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
            bool isOptionsNull = options.Distinct().Count() == 1;

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
