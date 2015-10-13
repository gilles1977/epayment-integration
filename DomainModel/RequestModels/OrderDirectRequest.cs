using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModel.RequestModels
{
    public class OrderDirectRequest
    {
        public string PspId { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Pswd { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string CardNo { get; set; }
        public string Ed { get; set; }
        public string Cvc { get; set; }
        public string Operation { get; set; }
        public string Cn { get; set; }
        public string Pm { get; set; }
        public string Brand { get; set; }
        public string Alias { get; set; }
    }
}
