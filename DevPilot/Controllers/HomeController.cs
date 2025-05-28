using LaunchDarkly.Client;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DevPilot.Controllers
{
    public class HomeController : Controller
    {
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
            bool newFeatureEnabled = new LaunchDarkly.Client.LdClient("sdk-2d6d3a0c-2a0d-4d3c-8b6a-2a0d4d3c8b6a").BoolVariation("LDNewFeature", new User("user-key"));
            var featureManager = (FeatureManager)HttpContext.Application["FeatureManager"];
            try
            {
                if ( !newFeatureEnabled && await featureManager.IsEnabledAsync("NewFeature") )
                {
                    if (await featureManager.IsEnabledAsync("BetaFeature"))
                    {
                        return Redirect("~/UserList.aspx");
                    }
                    else
                    {
                        return Redirect("~/Error.asp");
                    }
                }
                else
                {
                    return RedirectToAction("Users", "Home");
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