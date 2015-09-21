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

namespace AssociationsIntro
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

            Mapper.CreateMap<Models.Manufacturer, Controllers.ManufacturerBase>();
            Mapper.CreateMap<Models.Manufacturer, Controllers.ManufacturerWithVehicles>();
            Mapper.CreateMap<Controllers.ManufacturerAdd, Models.Manufacturer>();

            Mapper.CreateMap<Models.Vehicle, Controllers.VehicleBase>();
            Mapper.CreateMap<Models.Vehicle, Controllers.VehicleWithManufacturer>();
            Mapper.CreateMap<Controllers.VehicleAdd, Models.Vehicle>();

        }
    }
}
