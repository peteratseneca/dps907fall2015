using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LocalSecurity.Models;
using LocalSecurity.Controllers;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LocalSecurity.ServiceLayer
{
    public class Identity_repo : Repository<ApplicationUser>
    {
        // Constructor
        public Identity_repo(ApplicationDbContext ds) : base(ds) { }

        public bool DeleteUser(string userName)
        {
            var applicationUser = _dbset.SingleOrDefault(u => u.UserName == userName);

            if (applicationUser == null)
            {
                return false;
            }
            else
            {
                while (applicationUser.Claims.Count > 0)
                {
                    var claim = applicationUser.Claims.First();
                    applicationUser.Claims.Remove(claim);
                    _ds.SaveChanges();
                }

                _dbset.Remove(applicationUser);
                _ds.SaveChanges();

                return true;
            }
        }

    }

}
