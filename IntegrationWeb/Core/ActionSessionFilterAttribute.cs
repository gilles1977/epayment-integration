using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel;

namespace IntegrationWeb.Core
{
    public class ActionSessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session!= null && session["user"] == null && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                session["user"] = ApiHelpers.Get<User>("api/userauth/user?username=" + filterContext.HttpContext.User.Identity.Name);
                filterContext.Result = new RedirectResult("~/Home/TimeoutRedirect");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}