using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialLoginWebApp.Startup))]
namespace SocialLoginWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
