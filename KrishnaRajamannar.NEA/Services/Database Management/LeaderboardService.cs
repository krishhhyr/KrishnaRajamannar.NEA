using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services.Database
{
    public class LeaderboardService : ILeaderboardService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        // This is used to select the top 10 users with the highest number of points
        public IList<LeaderboardModel> GetLeaderboard()
        {
            IList<LeaderboardModel> leaderboard = new List<LeaderboardModel>();

            int rank = 1;
            string username = "";
            int numberOfPoints = 0;

            const string sqlQuery =
                @"
                    SELECT TOP 10 username, MAX(numberofpoints) 
                    From UserDetails
                    GROUP BY username,numberOfPoints
                    Order By numberofpoints DESC 
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var data = command.ExecuteReader();
            while (data.Read())
            {
                username = data.GetString(0);
                numberOfPoints = data.GetInt32(1);

                leaderboard.Add(new LeaderboardModel() { Rank = rank, Username = username, NumberOfPoints = numberOfPoints });
                rank++;
            }
            return leaderboard;
        }
    }
}
