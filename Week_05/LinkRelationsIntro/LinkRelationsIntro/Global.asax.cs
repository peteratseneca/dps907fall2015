﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
// new...
using AutoMapper;

namespace LinkRelationsIntro
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // AutoMapper maps...

            Mapper.CreateMap<Models.Vehicle, Controllers.VehicleBase>();
            Mapper.CreateMap<Controllers.VehicleAdd, Models.Vehicle>();
            Mapper.CreateMap<Controllers.VehicleBase, Controllers.VehicleWithLink>();
        }
    }
}
