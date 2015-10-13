using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegrationWeb.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult UnexpectedError()
        {
            return View();
        }
    }
}
