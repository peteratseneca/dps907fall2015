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

namespace MediaUpload
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

            // Attention - Add ByteFormatter to the pipeline
            GlobalConfiguration.Configuration.Formatters.Add(new MediaUpload.ServiceLayer.ByteFormatter());

            // AutoMapper maps...

            Mapper.CreateMap<Models.ExceptionInfo, Controllers.ExceptionInfoBase>();
            Mapper.CreateMap<Controllers.ExceptionInfoAdd, Models.ExceptionInfo>();
            Mapper.CreateMap<Controllers.ExceptionInfoBase, Controllers.ExceptionInfoWithLink>();

            // Attention - More AutoMapper maps
            Mapper.CreateMap<Models.Book, Controllers.BookBase>();
            Mapper.CreateMap<Models.Book, Controllers.BookWithMediaInfo>();
            Mapper.CreateMap<Controllers.BookAdd, Models.Book>();
            Mapper.CreateMap<Controllers.BookBase, Controllers.BookWithLink>();
            Mapper.CreateMap<Controllers.BookWithMediaInfo, Controllers.BookWithLink>();
        }

    }

}
