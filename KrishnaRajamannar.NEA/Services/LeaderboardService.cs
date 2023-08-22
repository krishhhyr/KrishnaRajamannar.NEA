using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    public class LeaderboardService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public LeaderboardService()
        {

        }

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
                if (rank > 10)
                {
                    break;
                }
                username = data.GetString(0);
                numberOfPoints = data.GetInt32(1);

                leaderboard.Add(new LeaderboardModel() { Rank = rank, Username = username, NumberOfPoints = numberOfPoints });
                rank++;
            }
            return leaderboard;
        }
    }
}
