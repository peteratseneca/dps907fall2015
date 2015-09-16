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

namespace AllHttpMethods
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

            Mapper.CreateMap<Models.Human, Controllers.HumanBase>();
            Mapper.CreateMap<Models.Human, Controllers.HumanList>();

            Mapper.CreateMap<Controllers.HumanAdd, Models.Human>();
            Mapper.CreateMap<Controllers.HumanEdit, Models.Human>();
        }
    }
}
