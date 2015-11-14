using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using System.Threading.Tasks;

namespace ClientAppInstruments.Controllers
{
    public class HomeController : Controller
    {
        // App's manager class reference
        Manager m = new Manager();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Login
        public ActionResult Login()
        {
            var form = new InstrumentAppCredentials();
            return View(form);
        }

        // POST: Home/Login
        [HttpPost]
        public async Task<ActionResult> Login(InstrumentAppCredentials credentials, string returnUrl)
        {
            // Validate the input
            if (!ModelState.IsValid) { return View(); }

            // Validate the credentials
            var isAuthenticated = await m.Login(credentials);
            if (!isAuthenticated)
            {
                // Configure the validation summary with an error message
                ModelState.AddModelError("", "Invalid credentials");
                // Display the form again
                return View(credentials);
            }

            // Credentials have been validated; return to the requested resource
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

    }

}
