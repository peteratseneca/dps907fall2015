using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using Lab6.Models;
using Lab6.Controllers;

namespace Lab6.ServiceLayer
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
