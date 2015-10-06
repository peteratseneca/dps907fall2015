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

namespace LinkRelationsMore
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

            Mapper.CreateMap<Models.Manufacturer, Controllers.ManufacturerBase>();
            Mapper.CreateMap<Models.Manufacturer, Controllers.ManufacturerFull>();
            Mapper.CreateMap<Controllers.ManufacturerBase, Controllers.ManufacturerWithLink>();
            Mapper.CreateMap<Controllers.ManufacturerFull, Controllers.ManufacturerWithLink>();
            Mapper.CreateMap<Models.Manufacturer, Controllers.ManufacturerFull>();
            Mapper.CreateMap<Controllers.ManufacturerAdd, Models.Manufacturer>();

            Mapper.CreateMap<Models.Vehicle, Controllers.VehicleBase>();
            Mapper.CreateMap<Models.Vehicle, Controllers.VehicleFull>();
            Mapper.CreateMap<Controllers.VehicleBase, Controllers.VehicleWithLink>();
            Mapper.CreateMap<Controllers.VehicleFull, Controllers.VehicleWithLink>();
            Mapper.CreateMap<Controllers.VehicleAdd, Models.Vehicle>();

        }
    }
}
