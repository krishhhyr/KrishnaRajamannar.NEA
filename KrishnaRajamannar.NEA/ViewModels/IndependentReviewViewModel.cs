using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event ShowMessageEventHandler ShowMessage;

        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewFeedbackWindow;

        public event HideWindowEventHandler HideIndependentReviewQuizWindow;

        private readonly IIndependentReviewQuizService _independentReviewQuizService;

        private readonly IUserService _userService;

        public IndependentReviewQuizFeedbackViewModel independentReviewQuizFeedbackViewModel;

        public int QuizID;
        public int UserID;
        private int questionNumber = 0;

        public IndependentReviewViewModel(IIndependentReviewQuizService independentReviewQuizService, IUserService userService)
        {
            _independentReviewQuizService = independentReviewQuizService;
            _userService = userService;

            independentReviewQuizFeedbackViewModel = App.ServiceProvider.GetService(typeof(IndependentReviewQuizFeedbackViewModel)) as IndependentReviewQuizFeedbackViewModel;
            _userService = userService;
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
        private string _questionNumberText;
        public string QuestionNumberText 
        {
            get { return _questionNumberText; }
            set 
            {
                _questionNumberText = value;
                RaisePropertyChange("QuestionNumberText");
            }
        }
        private string _correctAnswer;
        public string CorrectAnswer 
        {
            get { return _correctAnswer; }
            set 
            {
                _correctAnswer = value;
                RaisePropertyChange("CorrectAnswer");
            }
        }
        private string _answerInput;
        public string AnswerInput 
        {
            get { return _answerInput; }
            set 
            {
                _answerInput = value;
                RaisePropertyChange("AnswerInput");
            }
        }
        private int _pointsGained;
        public int PointsGained 
        {
            get { return _pointsGained; }
            set 
            {
                _pointsGained = value;
                RaisePropertyChange("PointsGained");
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
        private void HideIndependentReviewQuiz()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideIndependentReviewQuizWindow(args);
        }
        private void ShowIndependentReviewFeedback() 
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            args.QuizID = QuizID;
            OnShowIndependentReviewFeedbackWindow(args);
        }
        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = ShowMessage;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnHideIndependentReviewQuizWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideIndependentReviewQuizWindow;
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

        public void DisplayIndependentReviewFeedbackWindow() 
        {
            ShowIndependentReviewFeedback();
        }


        #region SortingQuestions
        public IList<IndependentReviewQuizModel> GetQuestionsInOrder() 
        {
            // Calls a function which returns all the questions for a particular quiz from the database.
            // These questions have not been sorted in order based on the number of points gained when reviewed by users. 
            IList<IndependentReviewQuizModel> unsortedquestions = _independentReviewQuizService.GetAllQuestions(QuizID);

            // A list for the number of points which can be initally gained by users when first reviewing a quiz.
            // This values were specified by users during the creation of questions.
            List<int> pointsForQuestion = new List<int>();

            // A loop which adds the number of points which could be gained from the questions retrieved into a list. 
            foreach (IndependentReviewQuizModel question in unsortedquestions) 
            {
                // I believe this should be points gained not points for question.
                pointsForQuestion.Add(question.PointsForQuestion);
            }

            // A merge sort algorithm which only sorts number of points gained from smallest to largest.
            List<int> sortedPoints = MSort(pointsForQuestion);

            // A list which is used to find the question associated with the number of points gained. 
            IList<IndependentReviewQuizModel> sortedquestions = new List<IndependentReviewQuizModel>();

            foreach (int point in sortedPoints) 
            {
                // Used to identify if a question has already been added to the list of sorted questions.
                IndependentReviewQuizModel recentQuestionAdded = new IndependentReviewQuizModel();

                foreach (IndependentReviewQuizModel question in unsortedquestions) //qs strawberry che 4, nissan 5, chocolate 4, kota 5, 17 3, 21st 1
                {
                    // If the point from the list of sortedPoints matches the number of points from the list of unsorted questions
                    // And the question has not already been added, add the question to the list of sorted questions. 
                    if ((point == question.PointsForQuestion) && (IsQuestionAdded(question.Question, sortedquestions) == false))
                    {
                        sortedquestions.Add(question);
                        recentQuestionAdded = question;
                    }
                }
            }
            return sortedquestions;
        }

        // Breaks down each element in the points list into individual elements. 
        public static List<int> MSort(List<int> points)
        {
            List<int> left, right;
            List<int> result = new List<int>(points.Count);

            if (points.Count <= 1) { return points; }

            int midpoint = points.Count / 2;

            left = new List<int>(midpoint);

            if ((points.Count % 2) == 0)
            {
                right = new List<int>(midpoint);
            }
            else { right = new List<int>(midpoint + 1); }

            for (int i = 0; i < midpoint; i++)
            {
                left.Add(points[i]);
            }

            for (int j = midpoint; j < points.Count; j++)
            {
                right.Add(points[j]);
            }

            left = MSort(left);
            right = MSort(right);

            result = Merge(left, right);
            return result;

        }

        // This merges the left-hand side of the list and the right-hand side of the list. 
        public static List<int> Merge(List<int> left, List<int> right)
        {
            int length = left.Count + right.Count;

            List<int> result = new List<int>(length);

            int leftIndex, rightIndex, resultIndex;
            leftIndex = rightIndex = resultIndex = 0;

            while ((leftIndex < left.Count) || (rightIndex < right.Count))
            {
                if ((leftIndex < left.Count) && (rightIndex < right.Count))
                {
                    if (left[leftIndex] <= right[rightIndex])
                    {
                        result.Add(left[leftIndex]);
                        leftIndex++;
                        resultIndex++;
                    }
                    else
                    {
                        result.Add(right[rightIndex]);
                        rightIndex++;
                        resultIndex++;
                    }
                }
                else if (leftIndex < left.Count)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                    resultIndex++;
                }
                else if (rightIndex < right.Count)
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                    resultIndex++;
                }
            }
            return result;
        }

        // Function which checks if a question has already been added to the list of sorted questions. 
        public bool IsQuestionAdded(string question, IList<IndependentReviewQuizModel> sortedQuestions) 
        {
            foreach (IndependentReviewQuizModel unsortedQuestion in sortedQuestions) 
            {
                if (unsortedQuestion.Question == question) 
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        // Used to send the current question being answered to the user interface.
        // If all the questions have been answered, the review session ends. 
        public void SendQuestion(IList<IndependentReviewQuizModel> questions) 
        {
            if (questionNumber >= questions.Count)
            {
                ShowMessageDialog("No more questions to review.");
                ShowMessageDialog($"You have gained {PointsGained} points in this review session.");

                _userService.UpdatePoints(UserID, PointsGained);

                ShowIndependentReviewFeedback();
                HideIndependentReviewQuiz();
            }
            else 
            {
                IndependentReviewQuizModel question = questions[questionNumber];
                Question = question.Question;
                questionNumber++;
                QuestionNumberText = $"Question: {questionNumber}/{questions.Count}";
            }
        }

        public bool IsQuestionTextBasedQuestion(IList<IndependentReviewQuizModel> questions) 
        {
            IndependentReviewQuizModel question = questions[questionNumber - 1];

            if (question.Option1 == "NULL")
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        // For a multiple-choice based question, this function sends the possible options to the user.
        public List<string?> SendOptions(IList<IndependentReviewQuizModel> questions)
        {
            IndependentReviewQuizModel question = questions[questionNumber - 1];

            List<string?> options = new List<string?>();

            options.Add(question.Option1);
            options.Add(question.Option2);
            options.Add(question.Option3);
            options.Add(question.Option4);
            options.Add(question.Option5);
            options.Add(question.Option6);

            return options;
        }

        // This function checks whether the user has inputted an answer to a question.
        // Also checks if the answer is correct or not.
        // Also calculates the total number of points gained for the review session. 
        public void ValidateAnswer(IList<IndependentReviewQuizModel> questions)
        {
            IndependentReviewQuizModel currentQuestion = questions[questionNumber - 1];

            // Checks if a user has not inputted an answer.
            if ((AnswerInput == "") || (AnswerInput == null))
            {
                MessageBox.Show("Enter a valid input.", "Independent Quiz Review");
            }
            else 
            {
                CorrectAnswer = currentQuestion.Answer;

                bool isCorrect = true;

                if (CorrectAnswer == AnswerInput)
                {
                    // Calls a function which calculates the number of points gained for the correct answer.
                    int pointsForCorrectAnswer = CalculatePoints(currentQuestion, isCorrect);

                    MessageBox.Show($"Correct. {pointsForCorrectAnswer} points have been awarded.", "Independent Quiz Review");
                    PointsGained = PointsGained + pointsForCorrectAnswer;

                    // Calls the IndependentReviewQuizService which uses an UPDATE command to update whether a question is correct 
                    // And how many points were gained.
                    _independentReviewQuizService.UpdateQuizFeedback(currentQuestion.FeedbackID, CalculateAnswerStreak(currentQuestion, isCorrect), isCorrect, pointsForCorrectAnswer);
                }
                // If the answer provided by the user is not correct. 
                else
                {
                    isCorrect = false;
                    int pointsForIncorrectAnswer = CalculatePoints(currentQuestion, isCorrect);

                    // Subtracts the number of points for the question from the total number of points gained. 
                    PointsGained = PointsGained - pointsForIncorrectAnswer;

                    _independentReviewQuizService.UpdateQuizFeedback(currentQuestion.FeedbackID, CalculateAnswerStreak(currentQuestion, isCorrect), isCorrect, -pointsForIncorrectAnswer);

                    // Used as the total number of points cannot be negative. 
                    if (PointsGained <= 0)
                    {
                        PointsGained = 0;
                        // Displays a message to the user indicating that the answer was wrong. 
                        MessageBox.Show($"Incorrect. 0 points have been awarded.", "Independent Quiz Review");

                        //return (correctAnswer, totalPoints);
                    }
                    if (PointsGained > 0)
                    {
                        MessageBox.Show($"Incorrect. {pointsForIncorrectAnswer} points have been deducted.", "Independent Quiz Review");
                    }
                }
            }
        }
        // Used to calculate the number of points which should be awarded to users. 
        public int CalculatePoints(IndependentReviewQuizModel question, bool isCorrect) 
        {
            IndependentReviewQuizModel currentQuestion = question;

            // If a question was consecutively answered incorrectly but is now correct. 
            if ((isCorrect == true) && (question.IsCorrect == false)) 
            {
                return currentQuestion.PointsForQuestion;
            }

            // If a question was consecutively answered correctly but is now incorrect.
            if ((isCorrect == false) && (question.IsCorrect == true))
            {
                return 0;
            }
            // If the question has not been answered beforehand (i.e if its a new question)
            // Then return the number of points specified by users when creating the question. 
            if ((currentQuestion.AnswerStreak == 0) || (currentQuestion.PointsGained == 0))
            {
                // Used so that the number of points increases based on how many times a question has been answered correctly. 


                //note: should be currentQuestion.PointsGained 
                //should just be currentquestion.answerstreak, no + 1, although if its a new question it will be + 1. 
                //because the answer streak is currently 0. 
                //int points = currentQuestion.PointsForQuestion * (currentQuestion.AnswerStreak + 1);

                return currentQuestion.PointsForQuestion;
            }
            // If the question has been answered beforehand and matches the answer when answered previously,
            // i.e if the question was first answered correctly and now is again answered correctly, 
            // return the multiplication of the answer streak and the number of points which were specified by the user during creation of the question.
            // This represents how if a question is answered consecutively, the number of points gained increases.
            else 
            { 
                return currentQuestion.PointsForQuestion * (currentQuestion.AnswerStreak + 1);
            }

            // Used so that the number of points increases based on how many times a question has been answered correctly. 
            //int points = currentQuestion.PointsForQuestion * (currentQuestion.AnswerStreak + 1);
            //return points;
        }
        // This is used to calculate the answer streak to a question.
        public int CalculateAnswerStreak(IndependentReviewQuizModel question, bool isCorrect) 
        {
            IndependentReviewQuizModel currentQuestion = question;

            // If a question has not been answered before. 
            if (currentQuestion.AnswerStreak == 0)
            {
                return currentQuestion.AnswerStreak + 1;
            }
            // If the answer does not match the previous response
            // If the answer was correct, but now is not correct
            // Reset the answer streak to 1.
            // This represents if the user has broken an answer streak to a question.
            else if (isCorrect != question.IsCorrect)
            {
                return 1;
            }
            else 
            {
                return currentQuestion.AnswerStreak + 1;
            }
        }
    }
}
