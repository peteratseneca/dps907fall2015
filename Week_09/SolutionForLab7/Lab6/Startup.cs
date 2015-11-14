using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
// new...
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(Lab6.Startup))]

namespace Lab6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Attention - Security infrastructure change

            // The following method is described in MSDN:
            // https://msdn.microsoft.com/en-us/library/owin.oauthbearerauthenticationextensions.useoauthbearerauthentication(v=vs.113).aspx
            // Adds Bearer token processing to an OWIN application pipeline.
            // This middleware understands appropriately formatted and secured tokens
            // which appear in the request header. The claims within the bearer token
            // are added to the current request's IPrincipal User.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions { });
        }
    }
}
