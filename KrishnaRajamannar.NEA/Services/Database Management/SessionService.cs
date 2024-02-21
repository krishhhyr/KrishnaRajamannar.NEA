using KrishnaRajamannar.NEA.Models.Dto;
using Microsoft.Data.SqlClient;
using System;

namespace KrishnaRajamannar.NEA.Services.Database
{
    public class SessionService : ISessionService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";
        // This is used to insert the data of a new session into the database.
        // This is used so that users can connect to the session by retrieving the IP address and the port number of the session they'd like to connect to
        public void InsertSessionData(int sessionID, string quiz, string endQuizCondition, string endQuizValue, string IPAddress, int portNumber, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO Session (SessionID, Quiz, EndQuizCondition, EndQuizValue, IPAddress, PortNumber, QuizID)
                    VALUES (@SessionID, @Quiz, @EndQuizCondition, @EndQuizValue, @IPAddress, @PortNumber, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", sessionID);
            command.Parameters.AddWithValue("Quiz", quiz);
            command.Parameters.AddWithValue("EndQuizCondition", endQuizCondition);
            command.Parameters.AddWithValue("EndQuizValue", endQuizValue);
            command.Parameters.AddWithValue("@IPAddress", IPAddress);
            command.Parameters.AddWithValue("@PortNumber", portNumber);
            command.Parameters.AddWithValue("@QuizID", quizID);
            command.ExecuteNonQuery();
            connection.Close();
        }

        // This uses a tuple to return two values from a function
        // The function is used to retrieve the IP address and the Port Number 
        // It's used so that users who want to join a session know which IP address
        // and port number to connect to
        public (string, int) GetConnectionData(int sessionID)
        {
            (string, int) connectionData;
            connectionData.Item1 = "";
            connectionData.Item2 = 0;
            const string sqlQuery =
                @"
                    SELECT IPAddress, PortNumber from Session
                    WHERE SessionID = @SessionID;
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", sessionID);
            var data = command.ExecuteReader();
            while (data.Read())
            {
                connectionData.Item1 = data.GetString(0);
                connectionData.Item2 = data.GetInt32(1);
            }
            return connectionData;
        }

        // This is used to check if a session ID already exists in the database
        // Used when generating new unique session IDs to prevent duplicates with existing session IDs
        public bool IsSessionIDExist(int sessionIDInput)
        {
            const string sqlQuery =
                @"
                    SELECT SessionID from Session
                    WHERE SessionID = @SessionID;
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", sessionIDInput);
            var data = command.ExecuteReader();
            if (data.Read() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Used to check if a port number generated is already being used in another session. 
        public bool IsPortNumberExist(int portNumberInput)
        {
            const string sqlQuery =
                @"
                    SELECT PortNumber from Session
                    WHERE PortNumber = @PortNumber;
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@PortNumber", portNumberInput);
            var data = command.ExecuteReader();
            if (data.Read() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // This is used to retrieve all the data about an existing session
        // After a user connects to the session, this data is passed over to the window of
        // the user who has just joined the session.

        // The data represents how the review of a quiz will end. 
        public SessionData GetSessionData(int sessionID)
        {
            SessionData sessionData = new SessionData();
            const string sqlQuery =
                @"
                    SELECT Quiz, EndQuizCondition, EndQuizValue FROM Session
                    WHERE SessionID = @SessionID;
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", sessionID);
            var data = command.ExecuteReader();
            while (data.Read())
            {
                sessionData.QuizSelected = data.GetString(0);
                sessionData.EndQuizCondition = data.GetString(1);
                sessionData.EndQuizConditionValue = Convert.ToString(data.GetInt32(2));
                
                return sessionData;
            }
            return sessionData;
        }
    }
}
