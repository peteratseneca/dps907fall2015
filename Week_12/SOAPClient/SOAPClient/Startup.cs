using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SOAPClient.Startup))]
namespace SOAPClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
