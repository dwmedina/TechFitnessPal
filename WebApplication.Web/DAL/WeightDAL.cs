using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
	public class WeightDAL : IWeightDAL
	{
		private readonly string connectionString;

		public WeightDAL(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public IList<UserWeight> GetWeights(User user, DateTime startDate, DateTime endDate)
		{
			List<UserWeight> weightsList = new List<UserWeight>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					SqlCommand cmd = new SqlCommand(
						"SELECT * FROM users " +
						"INNER JOIN dailyWeight ON users.userId = dailyWeight.userId " +
						"WHERE users.userId = @userId " +
						"AND dailyWeight.dateOfEntry >= @startDate " +
						"AND dailyWeight.dateOfEntry <= @endDate " +
						"ORDER BY dailyWeight.dateOfEntry ASC;", conn);

					cmd.Parameters.AddWithValue("@userId", user.Id);
					cmd.Parameters.AddWithValue("@startDate", startDate);
					cmd.Parameters.AddWithValue("@endDate", endDate);

					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						UserWeight userWeight = new UserWeight();

						userWeight.UserId = Convert.ToInt32(reader["userId"]);
						userWeight.TodaysWeight = Convert.ToInt32(reader["todaysWeight"]);
						userWeight.DateOfEntry = Convert.ToDateTime(reader["dateOfEntry"]);

						weightsList.Add(userWeight);
					}

				}
			}
			catch (SqlException ex)
			{
				throw ex;
			}

			return weightsList;
		}

		public void EnterDailyWeight(User user, int weight)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					SqlCommand cmd = new SqlCommand("SELECT * FROM users INNER JOIN dailyWeight ON users.userId = dailyWeight.userId WHERE users.userId = @userId AND dailyWeight.dateOfEntry = @today;", conn);
					cmd.Parameters.AddWithValue("@userId", user.Id);
					cmd.Parameters.AddWithValue("@today", DateTime.Today);

					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
					{
						conn.Close();
						conn.Open();

						SqlCommand updateCmd = new SqlCommand("UPDATE dailyWeight SET todaysWeight = @weight WHERE userId = @userId AND dateOfEntry = @today;", conn);
						updateCmd.Parameters.AddWithValue("@userId", user.Id);
						updateCmd.Parameters.AddWithValue("@today", DateTime.Today.ToShortDateString());
						updateCmd.Parameters.AddWithValue("@weight", weight);

						updateCmd.ExecuteNonQuery();
					}
					else
					{
						conn.Close();
						conn.Open();

						SqlCommand insertCmd = new SqlCommand("INSERT INTO dailyWeight (userId, dateOfEntry, todaysWeight) VALUES (@userId, @today, @dailyWeight);", conn);
						insertCmd.Parameters.AddWithValue("@userId", user.Id);
						insertCmd.Parameters.AddWithValue("@today", DateTime.Today.ToShortDateString());
						insertCmd.Parameters.AddWithValue("@dailyWeight", weight);

						insertCmd.ExecuteNonQuery();
					}

				}
			}
			catch (SqlException ex)
			{
				throw ex;
			}
		}

		public bool GetTodaysWeight(User user)
		{
			bool isWeightEnteredToday = false;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					SqlCommand cmd = new SqlCommand("SELECT * FROM users INNER JOIN dailyWeight ON users.userId = dailyWeight.userId WHERE users.userId = @userId AND dailyWeight.dateOfEntry = @today;", conn);
					cmd.Parameters.AddWithValue("@userId", user.Id);
					cmd.Parameters.AddWithValue("@today", DateTime.Today.ToShortDateString());

					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
					{
						isWeightEnteredToday = true;
					}
				}
			}
			catch (SqlException ex)
			{
				throw ex;
			}

			return isWeightEnteredToday;
		}
	}
}
