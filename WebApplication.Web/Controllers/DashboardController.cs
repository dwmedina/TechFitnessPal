using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Providers.Auth;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using System.Text;
using Newtonsoft.Json;
using WebApplication.Web.Extensions;


namespace WebApplication.Web.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IAuthProvider authProvider;
		private readonly IUserFoodDAL dal;
		private readonly IWeightDAL weightDal;
		private readonly IUserDAL userDal;
		private const string Session_Key = "Date_Range";
		
		public DashboardController(IAuthProvider authProvider, IUserFoodDAL dal, IWeightDAL weightDal, IUserDAL userDal)
		{
			this.authProvider = authProvider;
			this.dal = dal;
			this.weightDal = weightDal;
			this.userDal = userDal;
		}

		public IActionResult Index(DateTime? startDate, DateTime? endDate, int? weight)
		{
			var user = authProvider.GetCurrentUser();
			var userFoods = dal.GetUserFoods(user.Id);
			
			SavedDates dates = GetActiveSavedDates(startDate, endDate);
			TodaysWeight todaysWeight = GetTodaysWeight(weight, user);
			var userWeights = weightDal.GetWeights(user, dates.StartDate, dates.EndDate);

			var weightIsLogged = weightDal.GetTodaysWeight(user);
			if (!weightIsLogged)
			{
				ModelState.AddModelError("log-your-weight", "Enter today's weight");
			}

			if (user != null)
			{
				Tuple<User, IList<UserFood>, IList<UserWeight>, TodaysWeight, bool> data = new Tuple<User, IList<UserFood>, IList<UserWeight>, TodaysWeight, bool>(user, userFoods, userWeights, todaysWeight, weightIsLogged);

				var weightData = new List<int>();
				var dateData = new List<string>();

				foreach (var item in userWeights)
				{
					weightData.Add(item.TodaysWeight);
				}

				foreach (var date in userWeights)
				{
					dateData.Add(date.DateOfEntry.ToShortDateString());
				}

				ViewBag.AllUserWeights = weightData;
				ViewBag.AllUserDates = dateData;

				return View(data);
			}

			//Viewbag for chart data


			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public IActionResult SearchForFood()
		{
			return View();
		}

		[HttpGet]
		public IActionResult FoodResults(FoodPreview foodSearch)
		{
			ApiDAL api = new ApiDAL();
			string jsonRes = api.searchForFood(foodSearch.Name);


			JsonResponseModel jsonObj = JsonConvert.DeserializeObject<JsonResponseModel>(jsonRes);

			var brandedResults = jsonObj.branded;
			var commonResults = jsonObj.common;

			SearchResults res = new SearchResults();
			res.Name = foodSearch.Name;


			foreach (var i in brandedResults)
			{
				FoodPreview preview = new FoodPreview();
				preview.Name = i.food_name;
				preview.PhotoUrl = i.photo.thumb;
				preview.ServingQuantity = i.serving_qty;
				preview.ServingUnit = i.serving_unit;
				res.FoodSearchResults.Add(preview);
			}

			foreach (var i in commonResults)
			{
				FoodPreview preview = new FoodPreview();
				preview.Name = i.food_name;
				preview.PhotoUrl = i.photo.thumb;
				preview.ServingQuantity = i.serving_qty;
				preview.ServingUnit = i.serving_unit;
				res.FoodSearchResults.Add(preview);
			}

			return View(res);
		}

		[HttpGet]
		public IActionResult ViewFoodDetail(string name, string imgurl, string serving_unit, float serving_qty)
		{
			ApiDAL api = new ApiDAL();
			string jsonNutrition = api.getNutritionInfo(name);
			FoodItem foodItem = JsonConvert.DeserializeObject<FoodItem>(jsonNutrition);
			foodItem.foods[0].Name = name;
			foodItem.foods[0].Imgurl = imgurl;
			foodItem.foods[0].serving_qty = serving_qty;
			foodItem.foods[0].serving_unit = serving_unit;
			return View(foodItem);
		}

		[HttpGet]
		public IActionResult RecentFoods()
		{
			var user = authProvider.GetCurrentUser();
			IList<UserFood> recentUserFoods = new List<UserFood>();
			recentUserFoods = dal.GetRecentFoods(user.Id);

			return View(recentUserFoods);
		}

		[HttpPost]
		public IActionResult SaveFood(string name, string serving_unit, float serving_qty, int mealId, int numberOfServings)
		{
			User user = authProvider.GetCurrentUser();
			int userId = user.Id;

			ApiDAL api = new ApiDAL();
			string jsonNutrition = api.getNutritionInfo(name);
			FoodItem foodItem = JsonConvert.DeserializeObject<FoodItem>(jsonNutrition);
			Food food = foodItem.foods[0];
			food.Name = name;
			food.serving_unit = serving_unit;
			food.serving_qty = serving_qty;

			if (mealId == 0)
			{
				dal.SaveItemToUserFoodLog(userId, food);
			}
			else
			{
				dal.SaveItemToUserFoodLog(userId, food, mealId, numberOfServings);
			}


			return RedirectToAction("Index", "Dashboard");
		}

		[HttpGet]
		public IActionResult UpdateFood(int userId, int rowId, int mealId, int numberOfServings)
		{
			User user = authProvider.GetCurrentUser();
			dal.UpdateFood(user, userId, rowId, mealId, numberOfServings);

			return RedirectToAction("Index", "Dashboard");
		}

		[HttpGet]
		public IActionResult DeleteFoodItem(User user, int userId, int rowId)
		{
			user = authProvider.GetCurrentUser();
			dal.DeleteFoodItem(user, userId, rowId);

			return RedirectToAction("Index", "Dashboard");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult LogWeight(int weight)
		{
			User user = authProvider.GetCurrentUser();

			weightDal.EnterDailyWeight(user, weight);
			user.CurrentWeight = weight;
			userDal.UpdateUser(user);
			return RedirectToAction("Index", "Dashboard");
		}

		private SavedDates GetActiveSavedDates(DateTime? startDate, DateTime? endDate)
		{
			SavedDates dates = HttpContext.Session.Get<SavedDates>(Session_Key);

			// See if the user has a cart in session
			if (dates == null)
			{
				dates = new SavedDates();
				dates.StartDate = DateTime.Today.AddDays(-10);
				dates.EndDate = DateTime.Today;
				HttpContext.Session.Set(Session_Key, dates);
			}
			else if (startDate != null && endDate != null)
			{
				dates.StartDate = (DateTime)startDate;
				dates.EndDate = (DateTime)endDate;
				HttpContext.Session.Set(Session_Key, dates);
			}

			return dates;
		}

		private TodaysWeight GetTodaysWeight(int? weight, User user)
		{
			TodaysWeight todaysWeight = HttpContext.Session.Get<TodaysWeight>(Session_Key);

			// See if the user has a cart in session
			if (todaysWeight == null)
			{
				todaysWeight = new TodaysWeight();
				todaysWeight.Weight = user.CurrentWeight;
				HttpContext.Session.Set(Session_Key, todaysWeight);
			}
			else if (todaysWeight != null && weight != null)
			{
				todaysWeight.Weight = (int)weight;
				HttpContext.Session.Set(Session_Key, todaysWeight);
			}

			return todaysWeight;
		}
	}	
}