using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using Microsoft.AspNet.Identity.Owin;

namespace SecMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/UserList
        // Attention - This will fetch the list of registered users
        public ActionResult UserList()
        {
            // Container to hold the user names
            var userList = new List<string>();

            // Get a reference to the application's user manager
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Go through the users, and extract their names
            foreach (var user in userManager.Users)
            {
                userList.Add(user.UserName);
            }

            // Package it for the view
            ViewBag.UserList = userList;

            return View();
        }

    }

}
