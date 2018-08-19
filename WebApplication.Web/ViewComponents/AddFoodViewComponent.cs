using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Providers.Auth;


namespace WebApplication.Web.ViewComponents
{
    public class AddFoodViewComponent : ViewComponent
    {
		private readonly IAuthProvider authProvider;

		public AddFoodViewComponent(IAuthProvider authProvider)
		{
			this.authProvider = authProvider;
		}

		public IViewComponentResult Invoke()
		{
			var user = authProvider.GetCurrentUser();
			return View();
			
		}
		
	}
}

