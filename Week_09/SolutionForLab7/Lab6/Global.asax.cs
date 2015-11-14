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

namespace Lab6
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

            // Media type formatter for byte-oriented content
            GlobalConfiguration.Configuration.Formatters.Add(new ServiceLayer.ByteFormatter());

            // AutoMapper maps...

            // Instrument maps

            Mapper.CreateMap<Models.Instrument, Controllers.InstrumentBase>();
            Mapper.CreateMap<Models.Instrument, Controllers.InstrumentWithMediaInfo>();
            Mapper.CreateMap<Models.Instrument, Controllers.InstrumentWithMedia>();
            Mapper.CreateMap<Controllers.InstrumentAdd, Models.Instrument>();
            Mapper.CreateMap<Controllers.InstrumentBase, Controllers.InstrumentWithLink>();
            Mapper.CreateMap<Controllers.InstrumentWithMediaInfo, Controllers.InstrumentWithLink>();

            Mapper.CreateMap<Models.ExceptionInfo, Controllers.ExceptionInfoBase>();
            Mapper.CreateMap<Controllers.ExceptionInfoAdd, Models.ExceptionInfo>();
            Mapper.CreateMap<Controllers.ExceptionInfoBase, Controllers.ExceptionInfoWithLink>();
        }

    }

}
