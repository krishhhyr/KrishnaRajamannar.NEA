using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services.Database
{
    public class IndependentReviewQuizService : IIndependentReviewQuizService
    {
        private const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public IndependentReviewQuizService()
        {

        }

        //inital insert
        public void InsertTextQuestionQuizFeedback(int textQuestionID, int pointsForQuestion, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO QuizFeedback(TextQuestionID, MCQuestionID, PointsForQuestion, PointsGained, TimeTaken, IsCorrect, AnswerStreak, QuizID)
                    VALUES (@TextQuestionID, null, @PointsForQuestion, 0, 0, 0, 0, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@TextQuestionID", textQuestionID);
            command.Parameters.AddWithValue("@PointsForQuestion", pointsForQuestion);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            connection.Close();
        }

        //inital insert
        public void InsertMultipleChoiceQuestionQuizFeedback(int MCquestionID, int pointsForQuestion, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO QuizFeedback(TextQuestionID, MCQuestionID, PointsForQuestion, PointsGained, TimeTaken, IsCorrect, AnswerStreak, QuizID)
                    VALUES (null, @MCQuestionID, @PointsForQuestion, 0, 0, 0, 0, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@MCQuestionID", MCquestionID);
            command.Parameters.AddWithValue("@PointsForQuestion", pointsForQuestion);
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
            int feedbackID, pointsForQuestion, pointsGained, answerStreak;
            bool isCorrect;

            const string sqlQuery =
                @"
                    SELECT FeedbackID, Question, CorrectAnswer, Option1, Option2, Option3, Option4, Option5, Option6, NumberOfPoints as PointsForQuestion , PointsGained, IsCorrect, AnswerStreak 
                    FROM MultipleChoiceQuestion,QuizFeedback
                    WHERE MultipleChoiceQuestion.MCQuestionID = QuizFeedback.MCQuestionID 
                    And QuizFeedback.QuizID = @QuizID

                    UNION All

                    SELECT FeedbackID, Question, Answer, Null as Option1, null as Option2, null as Option3, null as Option4, null as Option5, null as Option6, NumberOfPoints as PointsForQuestion, PointsGained,  IsCorrect, AnswerStreak  
                    FROM TextQuestion,QuizFeedback
                    WHERE TextQuestion.TextQuestionID = QuizFeedback.TextQuestionID
                    And QuizFeedback.QuizID = @QuizID
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
                for (int i = 0; i < 6; i++)
                {
                    // Checks if the data in the database is null or not for an option.
                    // If it is not, it assigns the value to the respective option in the array.
                    if (data.IsDBNull(i + 3))
                    {
                        options[i] = "NULL";
                    }
                    else
                    {
                        options[i] = data.GetString(i + 3);
                    }
                }

                pointsForQuestion = data.GetInt32(9);

                if (data.IsDBNull(10))
                {
                    pointsGained = 0;
                }
                else
                {
                    pointsGained = data.GetInt32(10);
                }

                isCorrect = data.GetBoolean(11);

                answerStreak = data.GetInt32(12);

                questionsToReview.Add(new IndependentReviewQuizModel()
                {
                    FeedbackID = feedbackID,
                    Question = question,
                    Answer = answer,

                    Option1 = options[0],
                    Option2 = options[1],
                    Option3 = options[2],
                    Option4 = options[3],
                    Option5 = options[4],
                    Option6 = options[5],

                    PointsForQuestion = pointsForQuestion,
                    PointsGained = pointsGained,
                    IsCorrect = isCorrect,
                    AnswerStreak = answerStreak,
                });
            }

            return questionsToReview;
        }

        public IList<IndependentReviewQuizFeedbackModel> GetQuizFeedback(int quizID)
        {
            IList<IndependentReviewQuizFeedbackModel> quizFeedback = new List<IndependentReviewQuizFeedbackModel>();

            int questionNumber = 1;
            string question;
            bool isCorrect;

            const string sqlQuery =
                @"
                    SELECT Question, IsCorrect
                    FROM MultipleChoiceQuestion,QuizFeedback
                    WHERE MultipleChoiceQuestion.MCQuestionID = QuizFeedback.MCQuestionID 
                    AND QuizFeedback.QuizID = @QuizID

                    UNION All

                    SELECT Question, IsCorrect  
                    FROM TextQuestion,QuizFeedback
                    WHERE TextQuestion.TextQuestionID = QuizFeedback.TextQuestionID
                    AND QuizFeedback.QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            while (data.Read())
            {
                question = data.GetString(0);
                isCorrect = data.GetBoolean(1);

                quizFeedback.Add(new IndependentReviewQuizFeedbackModel() { QuestionNumber = questionNumber, Question = question, AnsweredCorrectly = isCorrect });
                questionNumber++;
            }
            return quizFeedback;
        }


        //update answer streak, update isCorrect to 1, update points 
        public void UpdateQuizFeedback(int feedbackID, int answerStreak, bool isCorrect, int pointsGained)
        {
            const string sqlQuery =
                @"
                    BEGIN TRANSACTION
                    UPDATE QuizFeedback
                    SET PointsGained = @PointsGained
                    WHERE FeedbackID = @FeedbackID

                    UPDATE QuizFeedback
                    SET AnswerStreak = @AnswerStreak
                    WHERE FeedbackID = @FeedbackID

                    UPDATE QuizFeedback
                    SET IsCorrect = @IsCorrect
                    WHERE FeedbackID = @FeedbackID
                    COMMIT;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@PointsGained", pointsGained);
            command.Parameters.AddWithValue("@AnswerStreak", answerStreak);
            command.Parameters.AddWithValue("@IsCorrect", isCorrect);
            command.Parameters.AddWithValue("@FeedbackID", feedbackID);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
