using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
	public class User
	{
		/// <summary>
		/// The user's id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The user's username.
		/// </summary>
		[Required]
		[MaxLength(50)]
		public string Username { get; set; }

		/// <summary>
		/// The user's email.
		/// </summary>
		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		/// <summary>
		/// The user's first name.
		/// </summary>
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		/// <summary>
		/// The user's last name.
		/// </summary>
		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		/// <summary>
		/// The user's birth date.
		/// </summary>
		[Required]
		public DateTime BirthDate { get; set; }

		/// <summary>
		/// The user's age.
		/// </summary>
		public int Age
		{
			get
			{
				int age = 0;
				age = DateTime.Now.Year - BirthDate.Year;
				if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
					age = age - 1;

				return age;
			}
		}

		/// <summary>
		/// The user's height (in inches).
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// The user's current weight (in lbs.).
		/// </summary>
		public int CurrentWeight { get; set; }

		/// <summary>
		/// The user's target/desired weight (in lbs.).
		/// </summary>
		public int DesiredWeight { get; set; }

		/// <summary>
		/// The recommended number of calories the user should consume daily.
		/// </summary>
		public int RecommendedDailyCaloricIntake
        {
            get
            {
                int recommendedDailyCalories = 0;
                int femaleBasalMetabolicRate =
                    (int) Math.Round(
                        (10 * (CurrentWeight * 2.20462)) +
                        (6.25 * (Height * 2.54)) - 
                        (5 * Age) - 161);

                // Note: ~200 cal difference between men / women
                int maleBasalMetabolicRate =
                    (int) Math.Round(
                        (10 * (CurrentWeight * 2.20462)) +
                        (6.25 * (Height * 2.54)) -
                        (5 * Age) + 5);

                // Assume the user lightly exercises (1 - 3 times per week)
                // These calculations are based on gaining and / or losing 1 lb per week

                // If the user wishes to maintain their current weight
                if (CurrentWeight == DesiredWeight)
                {
                    recommendedDailyCalories = (int) Math.Round(femaleBasalMetabolicRate * 1.375);
                }

                // If the user wishes to lose weight
                else if (CurrentWeight > DesiredWeight)
                {
                    recommendedDailyCalories = (int) Math.Round((femaleBasalMetabolicRate * 1.375) - 500);
                }

                // If the user wishes to gain weight
                else
                {
                    recommendedDailyCalories = (int) Math.Round((femaleBasalMetabolicRate * 1.375) + 500);
                }

                return recommendedDailyCalories;
            }
        }

		/// <summary>
		/// The number of consecutive meals that the user has logged.
		/// </summary>
		public int MealStreak { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The user's salt.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// The user's role.
        /// </summary>
        public string Role { get; set; }
    }
}
