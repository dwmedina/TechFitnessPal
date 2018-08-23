using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class TodaysWeight
    {
		public int Weight { get; set; }
		public DateTime TodaysDate = DateTime.Today;
    }
}
