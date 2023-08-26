using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    // A class which represents all the functions/procedures that the Quiz table has in the database
    public class QuizService : IQuizService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public QuizService()
        {
            
        }

        // Function which returns a list of quizzes for a particular user
        public IList<QuizModel> GetQuiz(int _userID)
        {
            IList<QuizModel> quizzes = new List<QuizModel>();

            int quizID;
            string quizTitle;
            int numberOfQuestions;
            int userID;

            const string sqlQuery =
               @"
                    SELECT Quiz.QuizID,Quiz.Title, Quiz.NumberOfQuestions, Quiz.UserID  
                    FROM Quiz, UserDetails
                    WHERE UserDetails.userID = Quiz.UserID AND UserDetails.userID = @UserID
               ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", _userID);

            var data = command.ExecuteReader();
            while (data.Read())
            {
                quizID = data.GetInt32(0);
                quizTitle = data.GetString(1);
                numberOfQuestions = data.GetInt32(2);
                userID = data.GetInt32(3);

                quizzes.Add(new QuizModel() { QuizID = quizID, QuizTitle = quizTitle, NumberOfQuestions = numberOfQuestions, UserID = userID });
            }
        
            return quizzes;
        }
        //Procedure which creates a quiz for a user based on a quiz title which has been inputted.
        public void CreateQuiz(int userID, string quizTitle) 
        {
            const string sqlQuery =
                @"
                    INSERT INTO Quiz(Title, UserID) 
                     VALUES (@QuizTitle, @UserID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@QuizTitle", quizTitle);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Procedure which deletes a quiz from a database.
        public void DeleteQuiz(int quizID, string quizTitle, int userID)
        {
            const string sqlQuery =
                @"
                    DELETE FROM QuizTextQuestionLink
                    WHERE QuizID = @QuizID;
                    DELETE FROM TextQuestion
                    WHERE QuizID = @QuizID;
                    DELETE FROM QuizMultipleChoiceQuestionLink
                    WHERE QuizID = @QuizID;
                    DELETE FROM MultipleChoiceQuestion
                    WHERE QuizID = @QuizID;
                    DELETE FROM Quiz 
                    WHERE Quiz.QuizID = @QuizID;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);
            command.Parameters.AddWithValue("@Title", quizTitle);
            command.Parameters.AddWithValue("@UserID", userID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Function which checks if a quiz already exists for a particular user..
        public bool IsQuizExists(int userID, string quizTitleInput) 
        {
            // Variable which represents the quiz title taken from the database.
            // If the quizTitleInput matches the quizTitleDB, the quiz already exists.
            string quizTitleDB;

            const string sqlQuery =
                @"
                    SELECT Quiz.Title 
                    FROM Quiz
                    WHERE UserID = @UserID AND Title = @QuizTitle
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@QuizTitle", quizTitleInput);

            var data = command.ExecuteReader();
            while (data.Read())
            {
                quizTitleDB = data.GetString(0);

                if (quizTitleDB == quizTitleInput)
                {
                    return true;
                }
            }
            // If there is no data in the database matching the quiz title, 
            // this means the quiz does not exist.
            if (data.Read() == false)
            {
                return false;
            }

            connection.Close();

            return false;
        }
        // Function which gets the Quiz ID for a quiz 
        public int? GetQuizID(int userID, string quizTitle) 
        {
            int quizID;

            const string sqlQuery =
                @"
                   SELECT Quiz.QuizID 
                   FROM Quiz 
                   WHERE UserID = @UserID AND Title = @Title 
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@Title", quizTitle);

            var data = command.ExecuteReader();
            while (data.Read())
            {
                quizID = data.GetInt32(0);
                return quizID;
            }
            connection.Close();

            return null;
        }
        public void UpdateNumberOfQuestions(int numberOfQuestions, int quizID) 
        {
            const string sqlQuery =
                @"
                    Update Quiz 
                    Set NumberOfQuestions = @NumberOfQuestions
                    Where QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@NumberOfQuestions", numberOfQuestions);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
