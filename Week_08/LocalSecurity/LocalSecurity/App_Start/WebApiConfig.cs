﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
// new...
using System.Web.Http.ExceptionHandling;

namespace LocalSecurity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // The custom error handling and logging classes must be registered here

            // Customized error handling and logging
            config.Services.Replace(typeof(IExceptionHandler), new ServiceLayer.HandleError());
            config.Services.Replace(typeof(IExceptionLogger), new ServiceLayer.LogError());

            // The HTTP OPTIONS handler must be registered here
            config.MessageHandlers.Add(new ServiceLayer.HandleHttpOptions());

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Added a default for "controller" so that we can use a root controller
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Root", id = RouteParameter.Optional }
            );
        }
    }
}
