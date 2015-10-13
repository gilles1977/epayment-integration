using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationWeb.Models
{
    public class FeedbackViewModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Pm { get; set; }
        public string Acceptance { get; set; }
        public string Status { get; set; }
        public string CardNo { get; set; }
        public string PayId { get; set; }
        public string NcError { get; set; }
        public string Brand { get; set; }
        public string Ed { get; set; }
        public string TrxDate { get; set; }
        public string Cn { get; set; }
        public string ShaSign { get; set; }
    }
}