using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services.Database
{
    public class UserSessionService : IUserSessionService
    {
        private IUserService _userService = new UserService();
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        // Used to insert the data into the UserSession table to keep a track of 
        // which user has joined which session.
        public void InsertUserSessionDetails(UserSessionData userSessiondata)
        {
            const string sqlQuery =
                @"
                    INSERT INTO UserSession (SessionID, UserID) 
                    VALUES (@SessionID, @UserID)
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", userSessiondata.SessionID);
            command.Parameters.AddWithValue("@UserID", userSessiondata.UserID);
            command.ExecuteNonQuery();
            connection.Close();
        }
        // Used to remove the data from the UserSession table 
        // once a user has disconnected from a session.
        public void RemoveUserSessionDetails(UserSessionData userSessiondata)
        {
            const string sqlQuery =
                @"
                    DELETE FROM UserSession
                    WHERE UserID = @UserID
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@UserID", userSessiondata.UserID);
            command.ExecuteNonQuery();
            connection.Close();
        }
        // Used to retrieve a list of objects (UserSessionData) consisting of the sessionID, userID and username
        // of users connected to a particular session.
        public IList<UserSessionData> GetUserSessionDetails(UserSessionData userData)
        {
            IList<UserSessionData> userSessiondata = new List<UserSessionData>();
            string sessionID = userData.SessionID;
            int userID;
            string username;
            const string sqlQuery =
                @"
                    SELECT UserID
                    FROM UserSession
                    WHERE SessionID = @SessionID
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", Convert.ToInt32(userData.SessionID));
            var data = command.ExecuteReader();
            while (data.Read())
            {
                userID = data.GetInt32(0);
                username = _userService.GetUsername(userID);
                userSessiondata.Add(new UserSessionData()
                {
                    SessionID = sessionID,
                    UserID = userID,
                    Username = username
                });
            }
            return userSessiondata;
        }
    }
}
