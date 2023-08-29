using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    public class IndependentReviewQuizService : IIndependentReviewQuizService 
    {
        private string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public IndependentReviewQuizService()
        {

        }

        //inital insert
        public void InsertTextQuestionQuizFeedback(int textQuestionID, int points, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO QuizFeedback(TextQuestionID, MCQuestionID, Points, TimeTaken, IsCorrect, AnswerStreak, QuizID)
                    VALUES (@TextQuestionID, null, @Points, 0, 0, 0, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@TextQuestionID", textQuestionID);
            command.Parameters.AddWithValue("@Points", points);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            connection.Close();
        }

        //inital insert
        public void InsertMultipleChoiceQuestionQuizFeedback(int MCquestionID, int points, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO QuizFeedback(TextQuestionID, MCQuestionID, Points, TimeTaken, IsCorrect, AnswerStreak, QuizID)
                    VALUES (null, @MCQuestionID, @Points, 0, 0, 0, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@MCQuestionID", MCquestionID);
            command.Parameters.AddWithValue("@Points", points);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public IList<IndependentReviewQuizModel> GetAllQuestions(int quizID) 
        {
            IList<IndependentReviewQuizModel> questionsToReview = new List<IndependentReviewQuizModel>();
            
            string question, answer;
            string? option1, option2, option3, option4, option5, option6;
            option1 = option2 = option3 = option4 = option5 = option6 = "";
            int feedbackID, pointsforquestion, score, answerStreak;
            bool isCorrect;

            const string sqlQuery =
                @"
                    SELECT FeedbackID, Question, CorrectAnswer, Option1, Option2, Option3, Option4, Option5, Option6, NumberOfPoints as PointsForQuestion ,Points as Score, IsCorrect, AnswerStreak FROM MultipleChoiceQuestion,QuizFeedback
                    WHERE MultipleChoiceQuestion.MCQuestionID = QuizFeedback.MCQuestionID 
                    And QuizFeedback.QuizID = 36

                    UNION All

                    SELECT FeedbackID, Question, Answer, Null as Option1, null as Option2, null as Option3, null as Option4, null as Option5, null as Option6, NumberOfPoints as PointsForQuestion, Points as Score,  IsCorrect, AnswerStreak  FROM TextQuestion,QuizFeedback
                    WHERE TextQuestion.TextQuestionID = QuizFeedback.TextQuestionID
                    And QuizFeedback.QuizID = 36
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            while (data.Read()) 
            {
                feedbackID = data.GetInt32(0);

                question = data.GetString(1);

                answer = data.GetString(2);

                string[]? options = { option1, option2, option3, option4, option5, option6 };
                for (int i = 1; i <= 6; i++)
                {
                    // Checks if the data in the database is null or not for an option.
                    // If it is not, it assigns the value to the respective option in the array.
                    if (data.IsDBNull(i + 1))
                    {
                        options[i - 1] = "NULL";
                    }
                    else
                    {
                        options[i - 1] = data.GetString(i + 1);
                    }
                }

                pointsforquestion = data.GetInt32(9);

                score = data.GetInt32(10);   

                isCorrect = data.GetBoolean(11);

                answerStreak = data.GetInt32(12);

                questionsToReview.Add(new IndependentReviewQuizModel() 
                {
                   FeedbackID = feedbackID ,Question = question, Answer = answer,

                    Option1 = options[0], Option2 = options[1], Option3 = options[2], 
                    Option4 = options[3], Option5 = options[4], Option6 = options[5],

                    PointsForQuestion = pointsforquestion, Points = score, IsCorrect = isCorrect, AnswerStreak = answerStreak,
                });
            }

            return questionsToReview;
        }


        //update answer streak, update isCorrect to 1, update points 
        public void UpdateCorrectAnswer() 
        {
            //const string 
        }
    }
}
