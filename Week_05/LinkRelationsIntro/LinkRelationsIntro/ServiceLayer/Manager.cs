using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using LinkRelationsIntro.Models;
using LinkRelationsIntro.Controllers;

namespace LinkRelationsIntro.ServiceLayer
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
