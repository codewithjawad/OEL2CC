using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.GoogleAutheticator;
using System.Web.Security;
using GoogleAuthentication.Models;
using System.Text;
using System.Web.Configuration;

namespace GoogleAuthentication.Controllers
{
    public class HomeController : Controller
    {
        // This method shows the main page only if the user is logged in and passed two-factor authentication
        public ActionResult Index()
        {
            if (Session["Username"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // This method shows the About page only if the user is logged in and passed two-factor authentication
        public ActionResult About()
        {
            if (Session["Username"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
            {
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Your application description page.";
            return View();
        }

        // This method shows the Contact page only if the user is logged in and passed two-factor authentication
        public ActionResult Contact()
        {
            if (Session["Username"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
            {
                return RedirectToAction("Login");
            }
            ViewBag.Message = "Your contact page.";
            return View();
        }

        // This method clears any existing login session and shows the login page
        public ActionResult Login()
        {
            Session["UserName"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return View();
        }

		// This method handles the login form submission
		// It checks the username and password,
		// and sets up Google two-factor authentication
		[HttpPost]
		public ActionResult Login(LoginModel login)
		{
			bool status = false;

			if (login.UserName == "Admin" && login.Password == "12345")
			{
				Session["UserName"] = login.UserName;
				Session["IsValidTwoFactorAuthentication"] = true; // Bypass 2FA temporarily
				Session["UserUniqueKey"] = login.UserName + WebConfigurationManager.AppSettings["GoogleAuthKey"];

				// Bypass Google Auth setup and redirect directly to script security UI
				return RedirectToAction("Check", "Script");
			}
			else
			{
				ViewBag.Status = false;
				ViewBag.Message = "Invalid username or password";
				return View();
			}
		}


		// This helper method converts the user secret into bytes (for 2FA setup)
		//  private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32)
		//    => secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);

		// This method verifies the 2FA code entered by the user
		public ActionResult TwoFactorAuthenticate()
		{
			var token = Request["CodeDigit"];
			string UserUniqueKey = Session["UserUniqueKey"].ToString();

			//var tfa = new Google.Authenticator.TwoFactorAuthenticator();
			//bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);

			//if (isValid)
			//{
			//	Session["IsValidTwoFactorAuthentication"] = true;
			//	return RedirectToAction("Index");
			//}

			ViewBag.Message = "Google Two Factor PIN is expired or wrong";
			return RedirectToAction("Login");
		}


		// This method logs the user out by clearing session values and redirects to login
		public ActionResult Logoff()
        {
            Session["UserName"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return RedirectToAction("Login");
        }
    }
}
