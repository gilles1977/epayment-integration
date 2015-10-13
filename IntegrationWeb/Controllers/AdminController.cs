using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using DomainModel.RequestModels;
using IntegrationWeb.Core;
using IntegrationWeb.Models;

namespace IntegrationWeb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Orders()
        {
            var orders = await ApiHelpers.LoadAsync<Order>("api/orders");
            var users = await ApiHelpers.LoadAsync<User>("api/userauth/users");
            
            var model = new OrdersViewModel
                {
                    Orders = (from o in orders
                              join u in users on o.UserId equals u.UserId
                              select
                                  new OrderViewModel()
                                      {
                                          ClientName = u.FirstName,
                                          OrderId = o.OrderId,
                                          Currency = o.Currency,
                                          TotalOrderAmount = o.Amount,
                                          Quantity = o.Quantity,
                                          Status = GetOrderStatus(o)
                                      }).ToList()
                };
            return View(model);
        }

        [HttpPost, AuthorizeRole("Admin")]
        public async Task<ActionResult> DirectCapture(OrdersViewModel model)
        {
            var selected = from m in model.Orders
                           where m.Selected
                           select m;
            var orders = await ApiHelpers.LoadAsync<Order>("api/orders");
            
            foreach (var orderVm in selected)
            {
                var order = orders.FirstOrDefault(o => o.OrderId == orderVm.OrderId);
                if (order == null)
                {
                    ModelState.AddModelError("feedback", string.Format("Error while getting order: {0}", orderVm.OrderId));
                    return View("Orders");
                }
                var request = new MaintenanceDirectRequest
                {
                    Operation = "SAS",
                    PayId = order.PayId,
                    PspId = ConfigurationUtil.PspId,
                    UserId = ConfigurationUtil.DirectUserName,
                    Pswd = ConfigurationUtil.DirectPassword
                };
                var response = await ApiHelpers.PostMaintenanceDirectAsync(ConfigurationUtil.DirectMaintenance, request);
                if (!response.Status.StartsWith("9"))
                {
                    ModelState.AddModelError("feedback", string.Format("Error while processing order: {0}", OrderStatus.Instance[response.Status]));
                    return View("Orders", model);
                }
            }

            return RedirectToAction("Orders");
        }

        [HttpPost, AuthorizeRole("Admin")]
        public async Task<JsonResult> CaptureAsync(OrdersViewModel model)
        {
            var selected = from m in model.Orders
                           where m.Selected
                           select m;
            var orders = await ApiHelpers.LoadAsync<Order>("api/orders");

            foreach (var orderVm in selected)
            {
                var order = orders.FirstOrDefault(o => o.OrderId == orderVm.OrderId);
                if (order == null)
                {
                    return Json(new { ok = false, message = string.Format("Error while getting order: {0}", orderVm.OrderId) });
                }
                var request = new MaintenanceDirectRequest
                {
                    Operation = "SAS",
                    PayId = order.PayId,
                    PspId = ConfigurationUtil.PspId,
                    UserId = ConfigurationUtil.DirectUserName,
                    Pswd = ConfigurationUtil.DirectPassword
                };
                var response = await ApiHelpers.PostMaintenanceDirectAsync(ConfigurationUtil.DirectMaintenance, request);
                if (!response.Status.StartsWith("9"))
                {
                    return Json(new { ok = false, message = OrderStatus.Instance[response.Status] });
                }
            }

            return Json(new { ok = true });
        }
        
        private string GetOrderStatus(Order order)
        {
            var request = new OrderQueryRequest {PayId = order.PayId, PspId = ConfigurationUtil.PspId, UserId = ConfigurationUtil.DirectUserName, Pswd = ConfigurationUtil.DirectPassword};
            var response = ApiHelpers.GetOrderQuery(ConfigurationUtil.DirectQuery, request);
            return response.Status;
        }
    }
}
