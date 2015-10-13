using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DomainModel;

namespace IntegrationWeb.Core
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(string role)
        {
            Role = role;
        }

        public string Role { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session != null)
            {
                var user = httpContext.Session["user"] as User;

                if (user != null)
                {
                    return user.Role == Role && base.AuthorizeCore(httpContext);
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Errors", action = "Unauthorized" }));
                
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}