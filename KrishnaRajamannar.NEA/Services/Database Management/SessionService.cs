﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services.Database
{
    public record ConnectionData(string ipadress, int port);
    public class SessionService : ISessionService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";
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
        public string GetQuizSelectedForSession(int sessionID)
        {
            string quizSelected = "";
            const string sqlQuery =
                @"
                    SELECT Quiz FROM Session
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
                quizSelected = data.GetString(0);
                return quizSelected;
            }
            return quizSelected;
        }
    }
}