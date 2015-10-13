using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DomainModel
{
    [DataContract]
    public class Order
    {
        public Order()
        {
            TrxDate = DateTime.Now;
        }
        [DataMember]
        public string OrderId { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string PayId { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string CardNo { get; set; }
        [DataMember]
        public string CardBrand { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public DateTime TrxDate { get; set; }
        [DataMember]
        public int Quantity { get; set; }
    }
}
