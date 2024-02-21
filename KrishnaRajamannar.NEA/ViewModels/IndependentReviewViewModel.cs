using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System.Collections.Generic;
using System.ComponentModel;

namespace KrishnaRajamannar.NEA.ViewModels
{
    // Inherits the NotifyPropertyChanged interface
    public class IndependentReviewViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ShowQuizParameterWindowEventHandler ShowIndependentReviewFeedbackWindow;
        public event HideWindowEventHandler HideIndependentReviewQuizWindow;

        private readonly IIndependentReviewQuizService _independentReviewQuizService;
        private readonly IUserService _userService;

        public IndependentReviewQuizFeedbackViewModel independentReviewQuizFeedbackViewModel;

        public int QuizID;
        public int UserID;
        private int QuestionNumber = 0;
        // Used to keep track of the current question being reviewed in the quiz
        private IndependentReviewQuizModel CurrentQuestion;

        public IndependentReviewViewModel(IIndependentReviewQuizService independentReviewQuizService, IUserService userService)
        {
            _independentReviewQuizService = independentReviewQuizService;
            _userService = userService;

            independentReviewQuizFeedbackViewModel = App.ServiceProvider.GetService(typeof(IndependentReviewQuizFeedbackViewModel)) as IndependentReviewQuizFeedbackViewModel;
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
        // Binds with the UI
        // Used to display the number of the question in the quiz
        // that's currently being answered 
        private string _questionNumberInQuiz;
        public string QuestionNumberInQuiz 
        {
            get { return _questionNumberInQuiz; }
            set 
            {
                _questionNumberInQuiz = value;
                RaisePropertyChange("QuestionNumberInQuiz");
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
        // Used to retrieve the answer that a user has inputted
        // to a question
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

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                _option1 = value;
                RaisePropertyChange("Option1");
            }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set
            {
                _option2 = value;
                RaisePropertyChange("Option2");
            }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set
            {
                _option3 = value;
                RaisePropertyChange("Option3");
            }
        }

        private string _option4;
        public string Option4
        {
            get { return _option4; }
            set
            {
                _option4 = value;
                RaisePropertyChange("Option4");
            }
        }

        private string _option5;
        public string Option5
        {
            get { return _option5; }
            set
            {
                _option5 = value;
                RaisePropertyChange("Option5");
            }
        }

        private string _option6;
        public string Option6
        {
            get { return _option6; }
            set
            {
                _option6 = value;
                RaisePropertyChange("Option6");
            }
        }

        // Used to display how many points a user has gained
        // when answering questions
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
        public void HideIndependentReviewQuiz()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideIndependentReviewQuizWindow(args);
        }
        // An event which is used to show the Independent Quiz Review feedback window
        public void ShowIndependentReviewFeedback() 
        {
            ShowQuizParameterWindowEventArgs args = new ShowQuizParameterWindowEventArgs();
            args.IsShown = true;
            // Used to pass the Quiz ID and PointsGained from this form onto the ReviewFeedback window
            args.QuizID = QuizID;
            OnShowIndependentReviewFeedbackWindow(args);
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

        #region SortingQuestions
        // This sorts the questions in a quiz based on how well the questions have been previously answered in a previous review of a the quiz
        // If the quiz has not previously been reviewed, then the questions are displayed in the order they were retrieved from the database
        public IList<IndependentReviewQuizModel> GetQuestionsInOrder() 
        {
            // Calls a function which returns all the questions for a particular quiz from the database.
            // These questions have not been sorted in order based on the number of points gained when reviewed by users. 
            IList<IndependentReviewQuizModel> unsortedQuestions = _independentReviewQuizService.GetAllQuestions(QuizID);

            // A list for the number of points which can be initally gained by users when first reviewing a quiz.
            // This values were specified by users during the creation of questions.
            List<int> pointsForQuestion = new List<int>();

            // A loop which adds the number of points which could be gained from the questions retrieved into a list. 
            foreach (IndependentReviewQuizModel question in unsortedQuestions) 
            {
                // I believe this should be points gained not points for question.
                pointsForQuestion.Add(question.PointsGained);
            }

            // A list which is used to find the question associated with the number of points gained. 
            IList<IndependentReviewQuizModel> sortedQuestions = new List<IndependentReviewQuizModel>();

            // This is used to check whether an entire quiz has been answered before or not.
            int notAnsweredQuestionCount = 0;

            // If all the questions have PointsGained as 0 and the Answer Streak is 0,
            // this means that none of the questions have been answered before. 
            // Therefore, the quiz has not been answered before 
            foreach (IndependentReviewQuizModel question in unsortedQuestions)
            {
                if ((question.PointsGained == 0) && (question.AnswerStreak == 0))
                {
                    notAnsweredQuestionCount++;
                }
            }

            // If the quiz has not been answered before, the questions retrieved are returned 
            // There is no need to sort them as the sorting algorithm sorts questions based on previous responses
            // (i.e the points gained in previous attempts of the quiz)
            if (notAnsweredQuestionCount >= unsortedQuestions.Count)
            {
                sortedQuestions = unsortedQuestions;
            }
            else 
            {
                // A merge sort algorithm which only sorts number of points gained from smallest to largest.
                List<int> sortedPoints = MSort(pointsForQuestion);

                foreach (int point in sortedPoints)
                {
                    // Used to identify if a question has already been added to the list of sorted questions.
                    IndependentReviewQuizModel recentQuestionAdded = new IndependentReviewQuizModel();

                    foreach (IndependentReviewQuizModel question in unsortedQuestions) 
                    {
                        // If the point from the list of sortedPoints matches the number of points from the list of unsorted questions
                        // And the question has not already been added, add the question to the list of sorted questions. 
                        if ((point == question.PointsGained) && (IsQuestionAdded(question.Question, sortedQuestions) == false))
                        {
                            sortedQuestions.Add(question);
                            recentQuestionAdded = question;
                        }
                    }
                }
            }
            return sortedQuestions;
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

        // Checks if a question has already been added to the list of sorted questions. 
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
        // If all the questions have been answered, the review session ends
        // and the feedback window for the quiz is displayed.
        public void SendQuestion(IList<IndependentReviewQuizModel> questions) 
        {
            if (QuestionNumber >= questions.Count)
            {
                Message = ($"No more questions to review. You have gained {PointsGained} points in this review session.");

                // Updates the userDetails table in the DB with the amount of points earned
                _userService.UpdatePoints(UserID, PointsGained);

                ShowIndependentReviewFeedback();
                HideIndependentReviewQuiz();
            }
            else 
            {
                // Retrieves the current question in the quiz from the object
                IndependentReviewQuizModel question = questions[QuestionNumber];
                // Assigns this to CurrentQuestion
                CurrentQuestion = question;
                // Used to display question to users
                Question = question.Question;
                QuestionNumber++;
                // Used to display what question is currently being answered out of how many questions there are
                QuestionNumberInQuiz = $"{QuestionNumber}/{questions.Count}";
            }
        }

        // Checks the question type
        // If Option1 is null, this means it is a text-based question 
        public bool IsQuestionTextBasedQuestion() 
        {
            if (CurrentQuestion.Option1 == "NULL")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        // For a multiple-choice based question, this assigns the possible options to the radio buttons in the UI 
        public void SendOptions()
        {
            Option1 = CurrentQuestion.Option1;
            Option2 = CurrentQuestion.Option2;
            Option3 = CurrentQuestion.Option3;
            Option4 = CurrentQuestion.Option4;
            Option5 = CurrentQuestion.Option5;
            Option6 = CurrentQuestion.Option6;
        }

        // This function checks whether the user has inputted an answer to a question.
        // Also checks if the answer is correct or not.
        // Also calculates the total number of points gained for the review session. 
        public bool ValidateAnswer()
        {
            // Checks if a user has not inputted an answer.
            if ((AnswerInput == "") || (AnswerInput == null))
            {
                Message = "Enter a valid input.";
                return false;
            }
            else 
            {
                CorrectAnswer = CurrentQuestion.Answer;

                bool isCorrect = true;

                if (CorrectAnswer == AnswerInput)
                {
                    // Calls a function which calculates the number of points gained for the correct answer.
                    int pointsForCorrectAnswer = CalculatePoints(isCorrect);

                    Message = $"Correct answer. {pointsForCorrectAnswer} points have been awarded.";
                    PointsGained = PointsGained + pointsForCorrectAnswer;

                    // Calls the IndependentReviewQuizService which uses an UPDATE command to update whether a question is correct 
                    // And how many points were gained.
                    _independentReviewQuizService.UpdateQuizFeedback(CurrentQuestion.FeedbackID, CalculateAnswerStreak(isCorrect), isCorrect, pointsForCorrectAnswer);
                }
                // If the answer provided by the user is not correct. 
                else
                {
                    isCorrect = false;
                    int pointsForIncorrectAnswer = CalculatePoints(isCorrect);

                    // Subtracts the number of points for the question from the total number of points gained. 
                    PointsGained = PointsGained - pointsForIncorrectAnswer;

                    _independentReviewQuizService.UpdateQuizFeedback(CurrentQuestion.FeedbackID, CalculateAnswerStreak(isCorrect), isCorrect, -pointsForIncorrectAnswer);
                        
                    // Used as the total number of points cannot be negative. 
                    if (PointsGained <= 0)
                    {
                        PointsGained = 0;
                        // Displays a message to the user indicating that the answer was wrong. 
                        Message = $"Answer was {CorrectAnswer}. 0 points have been awarded.";

                        //return (correctAnswer, totalPoints);
                    }
                    if (PointsGained > 0)
                    {
                        Message = $"Answer was {CorrectAnswer}. {pointsForIncorrectAnswer} points have been deducted.";
                    }
                }
                return true;
            }
        }
        // Used to calculate the number of points which should be awarded to users. 
        public int CalculatePoints(bool isCorrect) 
        {
            // If the question has not been answered beforehand (i.e if its a new question)
            // Then return the number of points specified by users when creating the question. 
            if ((CurrentQuestion.AnswerStreak == 0) || (CurrentQuestion.PointsGained == 0))
            {
                return CurrentQuestion.PointsForQuestion;
            }
            // If a question was consecutively answered incorrectly but is now correct. 
            if ((isCorrect == true) && (CurrentQuestion.IsCorrect == false)) 
            {
                return 0;
            }

            // If a question was consecutively answered correctly but is now incorrect.
            if ((isCorrect == false) && (CurrentQuestion.IsCorrect == true))
            {
                return 0;
            }
            // If the question has been answered beforehand and matches the answer when answered previously,
            // i.e if the question was first answered correctly and now is again answered correctly, 
            // return the multiplication of the answer streak and the number of points which were specified by the user during creation of the question.
            // This represents how if a question is answered consecutively, the number of points gained increases.
            else 
            { 
                return CurrentQuestion.PointsForQuestion * (CurrentQuestion.AnswerStreak + 1);
            }
        }
        // This is used to calculate the answer streak to a question.
        public int CalculateAnswerStreak(bool isCorrect) 
        {
            // If a question has not been answered before. 
            if (CurrentQuestion.AnswerStreak == 0)
            {
                return CurrentQuestion.AnswerStreak + 1;
            }
            // If the answer does not match the previous response
            // If the answer was correct, but now is not correct
            // Reset the answer streak to 1.
            // This represents if the user has broken an answer streak to a question.
            else if (isCorrect != CurrentQuestion.IsCorrect)
            {
                return 1;
            }
            else 
            {
                return CurrentQuestion.AnswerStreak + 1;
            }
        }
    }
}
