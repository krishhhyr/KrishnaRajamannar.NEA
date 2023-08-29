using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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



        public IList<IndependentReviewQuizModel> GetQuestionsInOrder() 
        {
            IList<IndependentReviewQuizModel> unsortedquestions = _independentReviewQuizService.GetAllQuestions(36);

            List<int> pointsForQuestion = new List<int>();

            foreach (IndependentReviewQuizModel question in unsortedquestions) 
            {
                pointsForQuestion.Add(question.Points);
            }

            List<int> sortedPoints = MSort(pointsForQuestion);

            IList<IndependentReviewQuizModel> sortedquestions = new List<IndependentReviewQuizModel>();

            //foreach (IndependentReviewQuizModel question in unsortedquestions)
            //{

            //    //prevents duplicate questions
            //    IndependentReviewQuizModel recentQuestionAdded = new IndependentReviewQuizModel();
            //    foreach (int point in sortedPoints)
            //    {
            //        if ((point == question.Points) && (recentQuestionAdded != question)) 
            //        {
            //            sortedquestions.Add(question);
            //            recentQuestionAdded = question;
            //        }
            //    }
            //}

            foreach (int point in sortedPoints) // 1 3 4 4 5 5
            {
                IndependentReviewQuizModel recentQuestionAdded = new IndependentReviewQuizModel();

                foreach (IndependentReviewQuizModel question in unsortedquestions) //qs strawberry che 4, nissan 5, chocolate 4, kota 5, 17 3, 21st 1
                {
                    if ((point == question.Points) && (IsQuestionAdded(question.Question, sortedquestions) == false))
                    {
                        sortedquestions.Add(question);
                        recentQuestionAdded = question;
                    }
                }
            }
            return sortedquestions;

            //(recentQuestionAdded.Points != question.Points)
        }

        //checks if the question already exists in the sorted questions - prevents duplicates 
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



        public string SendQuestion(IList<IndependentReviewQuizModel> questions) 
        {
            if (questionNumber >= questions.Count)
            {
                MessageBox.Show("No more questions");
                // show quiz feedback
                return "END";
            }
            else 
            {
                IndependentReviewQuizModel currentQuestion = questions[questionNumber];
                questionNumber++;
                return currentQuestion.Question;
            }
        }
        public string SendQuestionNumber(IList<IndependentReviewQuizModel> questions)
        {
            if (questionNumber > questions.Count)
            {
                MessageBox.Show("You have answered all the questions");
                return "END";
                //display quiz feedback
            }

            return $"Question: {questionNumber}/{questions.Count}";
        }

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

            if (options.Distinct().Count() != -1) 
            {
                return options;
            }
            return options;
        }

        public string CompareTextAnswers(string answerInput, IList<IndependentReviewQuizModel> question)
        {
            IndependentReviewQuizModel currentQuestion = question[questionNumber - 1];

            string correctAnswer = currentQuestion.Answer;

            if (answerInput == "") 
            {
                MessageBox.Show("No answer has been inputted.", "Quiz Review");

                return "";
            }

            //if answer is correct; change isCorrect to true, update points, update answer streak (check if not 0), output points attained to user, calc total points

            if (correctAnswer == answerInput)
            {
                //is correct = 1, answer streak = as + 1, 

                return "Correct!";
            }
            else
            {
                return correctAnswer;
            }
        }
        public int CalculatePoints(IList<IndependentReviewQuizModel> question) 
        {
            IndependentReviewQuizModel currentQuestion = question[questionNumber - 1];
            int points = currentQuestion.Points * (currentQuestion.AnswerStreak + 1);
            return points;
        }
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

    }
}
