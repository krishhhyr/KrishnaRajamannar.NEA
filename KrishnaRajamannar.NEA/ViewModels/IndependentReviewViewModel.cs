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
        private readonly IIndependentReviewQuizService _independentReviewQuizService;
        private readonly IndependentReviewQuizModel _independentReviewQuizModel;

        private int questionNumber = 0;
        private int totalPoints = 0;


        public IndependentReviewViewModel(IIndependentReviewQuizService independentReviewQuizService, IndependentReviewQuizModel independentReviewQuizModel)
        {
            _independentReviewQuizService = independentReviewQuizService;
            _independentReviewQuizModel = independentReviewQuizModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // get questions DONE!
        // merge sort to sort qs based on points previously awarded DONE!
        // display questions, question number 
        // check if answer is correct
        // display correct answer + calculate no of points awarded
        // update no of points in database, update label for points
        // total number of points = use sum function. 
        // provide user feedback when quiz has ended - data grid?


        #region SortingQuestions
        public IList<IndependentReviewQuizModel> GetQuestionsInOrder() 
        {
            // Calls a function which returns all the questions for a particular quiz from the database.
            // These questions have not been sorted in order based on the number of points gained when reviewed by users. 
            IList<IndependentReviewQuizModel> unsortedquestions = _independentReviewQuizService.GetAllQuestions(36);

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

            //(recentQuestionAdded.Points != question.Points)
        }

        #region MergeSort
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
        #endregion

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

        #region SendDataToUI
        // Used to send the current question being answered to the user interface.
        // If all the questions have been answered, the review session ends. 
        public string SendQuestion(IList<IndependentReviewQuizModel> questions) 
        {
            // Checks if the question number is greater than the number of questions in the quiz. 
            if (questionNumber >= questions.Count)
            {
                MessageBox.Show("No more questions left to review.", "Quiz Review", MessageBoxButton.OK);
                // show quiz feedback
                return "";
            }
            else 
            {
                // Increments the questionNumber everytime that the function is called. 
                IndependentReviewQuizModel currentQuestion = questions[questionNumber];
                questionNumber++;
                return currentQuestion.Question;
            }
        }
        // Used to send what question the user is currently answering out of how many questions are in the quiz.
        public string SendQuestionNumber(IList<IndependentReviewQuizModel> questions)
        {
            if (questionNumber > questions.Count)
            {
                MessageBox.Show("You have answered all the questions");
                //display quiz feedback - new window
            }

            return $"Question: {questionNumber}/{questions.Count}";
        }

        // For a multiple-choice based question, this function sends the possible options to the user.
        public List<string?> SendOptions(IList<IndependentReviewQuizModel> questions)
        {
            IndependentReviewQuizModel currentQuestion = questions[questionNumber - 1];

            List<string?> options = new List<string?>();

            options.Add(currentQuestion.Option1);
            options.Add(currentQuestion.Option2);
            options.Add(currentQuestion.Option3);
            options.Add(currentQuestion.Option4);
            options.Add(currentQuestion.Option5);
            options.Add(currentQuestion.Option6);

            return options;
        }
        #endregion

        #region ValidationAndUpdatingFeedback
        // This function checks whether the user has inputted an answer to a question.
        // Also checks if the answer is correct or not.
        // Also calculates the total number of points gained for the review session. 
        public (string, int) ValidateAnswer(string answerInput, IList<IndependentReviewQuizModel> question)
        {
            IndependentReviewQuizModel currentQuestion = question[questionNumber - 1];

            string correctAnswer = currentQuestion.Answer;

            bool isCorrect = true;

            // Checks if a user has not inputted an answer.
            if (answerInput == "") 
            {
                MessageBox.Show("No answer has been inputted.", "Independent Quiz Review");
                
                return ("", 0);
            }

            // Checks if the answer inputted by the user matches the correct answer. 
            if (correctAnswer == answerInput)
            {
                // Calls a function which calculates the number of points gained for the correct answer.
                int pointsForCorrectAnswer = CalculatePoints(currentQuestion, isCorrect);

                MessageBox.Show($"Correct! You have been awarded {pointsForCorrectAnswer} points.", "Independent Quiz Review");
                totalPoints = totalPoints + pointsForCorrectAnswer;

                // Calls the IndependentReviewQuizService which uses an UPDATE command to update whether a question is correct 
                // And how many points were gained.
                _independentReviewQuizService.UpdateFeedback(currentQuestion.FeedbackID, CalculateAnswerStreak(currentQuestion, isCorrect), isCorrect, pointsForCorrectAnswer);
                return ("Correct!", totalPoints);
            }
            // If the answer provided by the user is not correct. 
            else 
            {
                isCorrect = false;
                int pointsForIncorrectAnswer = CalculatePoints(currentQuestion, isCorrect);

                // Subtracts the number of points for the question from the total number of points gained. 
                totalPoints = totalPoints - pointsForIncorrectAnswer;

                _independentReviewQuizService.UpdateFeedback(currentQuestion.FeedbackID, CalculateAnswerStreak(currentQuestion, isCorrect), isCorrect, -pointsForIncorrectAnswer);

                // Used as the total number of points cannot be negative. 
                if (totalPoints <= 0) 
                {
                    totalPoints = 0; 
                    // Displays a message to the user indicating that the answer was wrong. 
                    MessageBox.Show($"Incorrect answer! You have zero points.", "Independent Quiz Review");

                    return (correctAnswer, totalPoints);
                }

                if (totalPoints > 0)
                {
                    MessageBox.Show($"Incorrect answer! You have lost {pointsForIncorrectAnswer} points.", "Independent Quiz Review");
                    return (correctAnswer, totalPoints);
                }

                return (correctAnswer, totalPoints);
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
        #endregion
    }
}
