using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MaritimeApplication
{
    public class MvcApplication : HttpApplication
    {
        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
