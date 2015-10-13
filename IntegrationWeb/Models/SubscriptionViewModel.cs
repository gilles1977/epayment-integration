using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationWeb.Models
{
    public class SubscriptionViewModel
    {
        public string SubscriptionId { get; set; }
        public string SubscriptionAmount { get; set; }
        public string SubscriptionOrderId { get; set; }
        public string SubscriptionPeriodUnit { get; set; }
        public string SubscriptionPeriodNumber { get; set; }
        public string SubscriptionPeriodMoment { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public string SubscriptionStatus { get; set; }
        public string SubscriptionComment { get; set; }
    }
}