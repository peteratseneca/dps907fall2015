using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using ErrorHandling.Models;
using ErrorHandling.Controllers;

namespace ErrorHandling.ServiceLayer
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
