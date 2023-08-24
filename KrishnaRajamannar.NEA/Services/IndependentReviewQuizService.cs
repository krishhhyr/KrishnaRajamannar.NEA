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
    }
}
