using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
// new...
using AutoMapper;

namespace AssociationsOther
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

            // AutoMapper maps
            // These define mappings between 'design model' and 'resource model' classes
            // Remember, we never work with 'design model' classes in our controllers 
            // and end-user 'use cases'

            // Models to Controllers base
            // Controllers add to Models
            // Controllers base to Controllers with link

            Mapper.CreateMap<Models.Employee, Controllers.EmployeeBase>();
            Mapper.CreateMap<Controllers.EmployeeAdd, Models.Employee>();

            Mapper.CreateMap<Models.Address, Controllers.AddressBase>();
            Mapper.CreateMap<Controllers.AddressAdd, Models.Address>();

            Mapper.CreateMap<Models.JobDuty, Controllers.JobDutyBase>();
            Mapper.CreateMap<Controllers.JobDutyAdd, Models.JobDuty>();
        }
    }
}
