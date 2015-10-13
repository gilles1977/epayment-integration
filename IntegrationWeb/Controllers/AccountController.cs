using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common.Utils;
using DomainModel;
using DomainModel.Models;
using IntegrationWeb.Models;

namespace IntegrationWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userModel = new UserModel() {UserName = model.UserName, Password = CryptoUtils.HashPassword(model.Password)};

                //workaround for bug in FormsAuthentication.SetAuthCookie for async task
                HttpResponseMessage response = null;
                using (var client = new HttpClient {BaseAddress = new Uri(ConfigurationUtil.ApiUrl)})
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var task = client.PostAsJsonAsync("api/userauth/validate", userModel)
                                     .ContinueWith(responseTask =>
                                     {
                                         response = responseTask.Result;
                                     });
                    task.Wait();
                    if (response.IsSuccessStatusCode)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        
                        var cart = new Cart();
                        Session["cart"] = cart;
                        var userTask = response.Content.ReadAsAsync<User>()
                                               .ContinueWith(res =>
                                                   {
                                                       var user = res.Result;
                                                       Session["user"] = user;
                                                   });
                        userTask.Wait();
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "User name or password is incorrect");
                    }
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            var cart = Session["cart"] as Cart;
            if (cart != null)
            {
                cart.Clear();
            }
            Session["cart"] = cart;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
