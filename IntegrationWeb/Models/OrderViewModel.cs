using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationWeb.Models
{
    public class OrdersViewModel
    {
        public OrdersViewModel()
        {
            Orders = new List<OrderViewModel>();
        }

        public IList<OrderViewModel> Orders { get; set; }
        public bool Async { get; set; }
    }

    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string ClientName { get; set; }
        public string Currency { get; set; }
        public double TotalOrderAmount { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get { return DomainModel.OrderStatus.Instance[Status]; } }
        public string Status { get; set; }
        public bool Selected { get; set; }
        public bool Disabled {
            get { return Status != "5"; }
        }
    }
}