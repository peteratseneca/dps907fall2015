using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using ProjectWithSecurity.Models;
using ProjectWithSecurity.Controllers;

namespace ProjectWithSecurity.ServiceLayer
{
    public class Manager
    {
        private Worker w;

        public Manager(Worker worker)
        {
            w = worker;
        }

        // Add application and business logic here
    }

}
