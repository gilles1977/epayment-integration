using System.Web;
using System.Web.Mvc;
using IntegrationWeb.Core;

namespace IntegrationWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
            filters.Add(new ActionSessionFilterAttribute());
        }
    }
}