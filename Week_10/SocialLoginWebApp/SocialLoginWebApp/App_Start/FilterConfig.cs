using System.Web;
using System.Web.Mvc;

namespace SocialLoginWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());

            // optional - force ALL access to the app to be authenticated
            //filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
    }
}
