using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IWeightDAL
    {
		IList<UserWeight> GetWeights(User user, DateTime startDate, DateTime endDate);

		void EnterDailyWeight(User user, int weight);

		bool GetTodaysWeight(User user);
	}
}
