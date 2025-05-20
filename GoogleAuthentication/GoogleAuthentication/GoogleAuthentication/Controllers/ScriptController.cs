using GoogleAuthentication.Models;
using System;
using System.Web.Mvc;

namespace GoogleAuthentication.Controllers
{
	public class ScriptController : Controller
	{
		[HttpGet]
		public ActionResult Check()
		{
			if (!IsUserAuthenticated()) return RedirectToAction("Login", "Home");
			return View(new ScriptCheckModel());
		}

		[HttpPost]
		public ActionResult Check(ScriptCheckModel model)
		{
			if (!IsUserAuthenticated()) return RedirectToAction("Login", "Home");

			try
			{
				var service = new calculator.calculator();

				// Call the web method (returns string "Safe" or "Malicious")
				string result = service.IsScriptMalicious(model.ScriptContent);

				// Set the result in the view model
				model.Result = result;
			}
			catch (Exception ex)
			{
				model.Result = "Error: " + ex.Message;
			}

			return View(model);
		}

		private bool IsUserAuthenticated()
		{
			return Session["UserName"] != null &&
				   Session["IsValidTwoFactorAuthentication"] != null &&
				   (bool)Session["IsValidTwoFactorAuthentication"];
		}
	}
}
