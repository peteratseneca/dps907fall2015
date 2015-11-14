﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ClientAppInstruments
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_EndRequest()
        {
            // Handling error conditions...
            // Inspiration: http://stackoverflow.com/a/9026907 

            var code = Context.Response.StatusCode;

            // Add more conditions here as you need them
            if (code == 404) { this.HandleError("NotFound"); }
            if (code >= 500) { this.HandleError("ServerError"); }
        }

        private void HandleError(string action)
        {
            // This method causes the Errors controller to handle the request
            // It creates the controller, then executes the desired action method

            // With some more code, it could also save the error,
            // and notify the web site programmer(s)

            // Clear the accumulated data out of the response object
            Response.Clear();

            // Declare a controller
            IController c = null;

            // Create the route data configuration
            var rd = new RouteData();
            rd.Values["controller"] = "Errors";
            rd.Values["action"] = action;
            c = new Controllers.ErrorsController();

            // Execute the controller method
            c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
        }

    }

}
