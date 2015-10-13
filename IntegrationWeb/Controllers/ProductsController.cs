using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Net.Http;
using DomainModel;
using IntegrationWeb.Models;
using Common.Utils;
using IntegrationWeb.Core;
using DomainModel.RequestModels;
using System.Threading.Tasks;

namespace IntegrationWeb.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        #region GET Actions

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var model = new ProductsViewModel
                {
                    Cart = SessionCart,
                    Products = await ApiHelpers.LoadAsync<Product>("api/products"),
                    CurrentUser = Session["user"] as User
                };
            return View(model);
        }

        [CartSessionFilter]
        public async Task<ActionResult> CheckOut()
        {
            var model = await GetProductViewModelAsync();
            if (model.Cart.HasRecurringProducts ^ model.Cart.HasSplitProducts)
            {
                return View("RecurringOptin", model);
            }
            return View(model);
        }

        [CartSessionFilter]
        public async Task<ActionResult> DirectCheckOut()
        {
            var model = await GetProductViewModelAsync();
            var userWallet = (from w in await ApiHelpers.LoadAsync<Wallet>("api/wallet")
                              where w.UserId == model.CurrentUser.UserId
                              select w).ToList();
            if (userWallet.Any())
            {
                return View("PaymentMethod",
                            new PaymentMethodViewModel
                                {
                                    Wallets = userWallet,
                                    ReturnAction = "DirectCheckOut",
                                    AliasAction = "PreRegisterdCheckout"
                                });
            }
            return View(model);
        }

        [CartSessionFilter]
        public async Task<ActionResult> Success(FeedbackViewModel model)
        {
            var user = Session["user"] as User ?? new User();

            var order = new Order
            {
                Amount = SessionCart.Total,
                OrderId = model.OrderId,
                UserId = user.UserId,
                PayId = model.PayId,
                Status = model.Status,
                Currency = model.Currency,
                Quantity = SessionCart.Products.Count,
                TrxDate = DateTime.ParseExact(model.TrxDate, "MM/dd/yy", CultureInfo.InvariantCulture)
            };
            if (!await ApiHelpers.SaveAsync(order, "api/orders")) return RedirectToAction("UnexpectedError", "Errors");
            SessionCart.Clear();
            return View(order);
        }

        #endregion

        #region POST Actions

        [HttpPost]
        [CartSessionFilter]
        public async Task<ActionResult> PaymentMethod(PaymentMethodViewModel model)
        {
            var pm = await GetProductViewModelAsync();

            if (string.IsNullOrEmpty(model.ChosenId))
            {
                return View(model.ReturnAction, pm);
            }
            var userWallet = (from w in await ApiHelpers.LoadAsync<Wallet>("api/wallet")
                              where w.UserId == pm.CurrentUser.UserId
                              select w).FirstOrDefault(uw => uw.WalletId == model.ChosenId);
            pm.ChosenWallet = userWallet;
            return View(model.AliasAction, pm);
        }

        [HttpPost]
        [CartSessionFilter]
        public async Task<ActionResult> PreRegisterdCheckout(ProductsViewModel model)
        {
            model = await GetProductViewModelAsync(model);

            if (!ModelState.IsValid)
            {
                return View("PreRegisterdCheckout", model);
            }

            var request = new OrderDirectRequest
                {
                    Alias = model.ChosenWallet.WalletId,
                    Amount = model.Amount,
                    Currency = ConfigurationUtil.DefaultCurrency,
                    Cvc = model.Cvc,
                    Operation = ConfigurationUtil.DefaultOperation,
                    OrderId = model.OrderId,
                    PspId = ConfigurationUtil.PspId,
                    Pswd = ConfigurationUtil.DirectPassword,
                    UserId = ConfigurationUtil.DirectUserName,
                    Pm = model.PaymentMethod,
                    Brand = model.Brand
                };

            var response = await ApiHelpers.PostOrderDirectAsync(ConfigurationUtil.DirectOrder, request);
            if (response.Status != "5")
            {
                ModelState.AddModelError("feedback",
                                         string.Format("Error while processing order: {0}",
                                                       OrderStatus.Instance[response.Status]));
                return View(model);
            }

            var order = new Order
                {
                    Amount = model.Cart.Total,
                    OrderId = model.OrderId,
                    UserId = model.CurrentUser.UserId,
                    CardBrand = response.Brand,
                    CardNo = response.CardNo,
                    Currency = ConfigurationUtil.DefaultCurrency,
                    PayId = response.PayId,
                    Quantity = model.Cart.Products.Count,
                    Status = response.Status,
                    TrxDate = DateTime.Now
                };
            if (!await ApiHelpers.SaveAsync(order, "api/orders")) return RedirectToAction("UnexpectedError", "Errors");

            SessionCart.Clear();

            return View("Success", order);
        }

        [HttpPost]
        [CartSessionFilter]
        public async Task<ActionResult> DirectCheckOut(ProductsViewModel model)
        {
            model = await GetProductViewModelAsync(model);

            if (!ModelState.IsValid)
            {
                return View("DirectCheckout", model);
            }

            var request = new OrderDirectRequest
                {
                    Amount = model.Amount,
                    CardNo = model.CardNumber,
                    Currency = ConfigurationUtil.DefaultCurrency,
                    Cvc = model.Cvc,
                    Ed = model.ExpiryDate,
                    Operation = ConfigurationUtil.DefaultOperation,
                    OrderId = model.OrderId,
                    PspId = ConfigurationUtil.PspId,
                    Pswd = ConfigurationUtil.DirectPassword,
                    UserId = ConfigurationUtil.DirectUserName,
                    Cn = model.CurrentUser.FullName,
                    Pm = model.PaymentMethod,
                    Brand = model.Brand
                };

            if (model.StoreFinancialData)
            {
                var wallet = CreateWallet(model);
                if (!await ApiHelpers.SaveAsync(wallet, "api/wallet")) return RedirectToAction("UnexpectedError", "Errors");
                request.Alias = wallet.WalletId;
            }

            var response = await ApiHelpers.PostOrderDirectAsync(ConfigurationUtil.DirectOrder, request);
            if (response.Status != "5")
            {
                ModelState.AddModelError("feedback",
                                         string.Format("Error while processing order: {0}",
                                                       OrderStatus.Instance[response.Status]));
                return View(model);
            }

            var order = new Order
                {
                    Amount = model.Cart.Total,
                    OrderId = model.OrderId,
                    UserId = model.CurrentUser.UserId,
                    CardBrand = response.Brand,
                    CardNo = response.CardNo,
                    Currency = response.Currency,
                    PayId = response.PayId,
                    Quantity = model.Cart.Products.Count,
                    Status = response.Status,
                    TrxDate = DateTime.Now
                };

            if (!await ApiHelpers.SaveAsync(order, "api/orders")) return RedirectToAction("UnexpectedError", "Errors");

            SessionCart.Clear();

            return View("Success", order);
        }

        [HttpPost]
        [CartSessionFilter]
        public async Task<ActionResult> RecurringOptin(ProductsViewModel model)
        {
            // If cart contains neither recurring subscribed nor split payments
            if (!model.Cart.HasRecurringSubscription && !model.Cart.HasInstallements)
            {
                return RedirectToAction("CheckOut");
            }

            foreach (var product in model.Cart.Products.Where(p => p.Recurring))
            {
                SessionCart[product.CartProductId].Recurring = product.Recurring;
                SessionCart[product.CartProductId].RecurringPeriod = product.Recurring ? "m" : "";
            }

            foreach (var product in model.Cart.Products.Where(p => p.InstallementNumber > 0))
            {
                SessionCart[product.CartProductId].InstallementNumber = product.InstallementNumber;
            }

            model = await GetProductViewModelAsync();
            var userWallet = (from w in await ApiHelpers.LoadAsync<Wallet>("api/wallet")
                              where w.UserId == model.CurrentUser.UserId
                              select w).ToList();
            if (userWallet.Any())
            {
                return View("PaymentMethod",
                            new PaymentMethodViewModel
                                {
                                    Wallets = userWallet,
                                    ReturnAction = "RecurringOptin",
                                    AliasAction = "SubscriptionCheckout",
                                    CannotChooseOther = true
                                });
            }
            return View(model);
        }

        #endregion

        #region Ajax Actions

        [HttpPost]
        public async Task<JsonResult> Add(int id)
        {
            try
            {
                var products = await ApiHelpers.LoadAsync<Product>("api/products");
                var orderedProduct = new OrderedProduct(products.FirstOrDefault(p => p.ProductId == id));

                SessionCart.Add(orderedProduct);

                return Json(new {ok = true, itemcount = SessionCart.ItemCount, totalcart = SessionCart.Total});
            }
            catch (Exception ex)
            {
                //TODO Log exception
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            try
            {
                SessionCart.Remove(id);
                return Json(new {ok = true, itemcount = SessionCart.ItemCount, totalcart = SessionCart.Total});
            }
            catch (Exception ex)
            {
                //TODO Log exception
                return Json(null);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetShaSignature(FormCollection form)
        {
            var passPhrase = ConfigurationUtil.PassPhrase;
            var stringToHash = string.Join("", from key in form.AllKeys
                                               where !string.IsNullOrEmpty(form[key]) && key != "__RequestVerificationToken"
                                               orderby key.Replace("_", "Z").ToUpperInvariant()
                                               select
                                                   string.Format("{0}={1}{2}", key.ToUpperInvariant(), form[key], passPhrase));
            var hash = CryptoUtils.GetSha1Signature(stringToHash);

            return Json(new {ok = true, str = stringToHash, hash = hash});
        }

        #endregion

        #region Private Helpers

        private async Task<ProductsViewModel> GetProductViewModelAsync(ProductsViewModel model = null)
        {
            var user = Session["user"] as User;
            if (model == null)
            {
                model = new ProductsViewModel();
            }
            model.Cart = SessionCart;
            model.Products = await ApiHelpers.LoadAsync<Product>("api/products");
            model.CurrentUser = user;
            model.OrderId = string.Format("{0:X}", DateTime.Now.Ticks.GetHashCode());
            model.Amount = string.Format("{0:0}", Math.Round(SessionCart.Total*100, 0));
            model.PaymentMethod = "CreditCard";
            SessionCart.OrderId = model.OrderId;
            if (SessionCart.HasRecurringSubscription)
            {
                model.Subscription = new SubscriptionViewModel
                    {
                        SubscriptionId = string.Format("SUB{0}", model.OrderId),
                        SubscriptionAmount = string.Format("{0:#}", Math.Round(SessionCart.TotalSubscriptions*100, 0)),
                        SubscriptionComment = "",
                        SubscriptionOrderId = string.Format("SUBORD{0}", model.OrderId),
                        SubscriptionPeriodUnit = "m",
                        SubscriptionPeriodNumber = "1",
                        SubscriptionPeriodMoment = "1",
                        SubscriptionStartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        SubscriptionEndDate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"),
                        SubscriptionStatus = "1"
                    };
            }
            if (SessionCart.HasInstallements)
            {
                model.PaymentViewModel = new ScheduledPaymentViewModel()
                    {
                        Amount1 = string.Format("{0:#}", Math.Round(SessionCart.TotalScheduledPayments*100, 0)),
                        Amount2 = string.Format("{0:#}", Math.Round(SessionCart.TotalScheduledPayments*100, 0)),
                        Amount3 = string.Format("{0:#}", Math.Round(SessionCart.TotalScheduledPayments*100, 0)),
                        ExecutionDate2 = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy"),
                        ExecutionDate3 = DateTime.Now.AddMonths(2).ToString("dd/MM/yyyy")
                    };
            }
            return model;
        }

        private Wallet CreateWallet(ProductsViewModel productsViewModel)
        {
            return new Wallet
                {
                    WalletId = string.Format("{0:X}", DateTime.Now.Ticks.GetHashCode()),
                    Brand = productsViewModel.Brand,
                    CardHolderName = productsViewModel.CardHolderName,
                    HiddenCardNumber = string.Format("XXXXXXXXXXXX{0}", productsViewModel.CardNumber.Substring(12)),
                    ExpiryDate = productsViewModel.ExpiryDate,
                    PaymentMethod = productsViewModel.PaymentMethod,
                    UserId = productsViewModel.CurrentUser.UserId
                };
        }

        private Cart _sessionCart;
        private Cart SessionCart
        {
            get
            {
                if (_sessionCart == null)
                {
                    _sessionCart = Session["cart"] as Cart ?? new Cart();
                    Session["cart"] = _sessionCart;
                }
                return _sessionCart;
            }
        }

        #endregion
    }
}
