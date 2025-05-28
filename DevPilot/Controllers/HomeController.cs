using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DevPilot.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Users()
        {
            var featureManager = (FeatureManager)HttpContext.Application["FeatureManager"];
            try
            {
                if (await featureManager.IsEnabledAsync("NewFeature"))
                {
                    return Redirect("~/UserList.aspx");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Redirect("~/Error.asp");
            }
        }
        public async Task<ActionResult> Features()
        {
            var featureManager = (FeatureManager)HttpContext.Application["FeatureManager"];
            try
            {
                if (await featureManager.IsEnabledAsync("NewFeatureTest"))
                {
                    return Redirect("~/UserList.aspx");
                }
                else
                {
                    return Redirect("~/Error.asp");
                }
            }
            catch (Exception ex)
            {
                return Redirect("~/Error.asp");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                if (username == "admin" && password == "password")
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}