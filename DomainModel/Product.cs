using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
    [DataContract]
    public class Product
    {
        [DataMember(Name = "id")]
        public int ProductId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "price"), DisplayFormat(DataFormatString = "{0:0.00#}")]
        public double Price { get; set; }
        [DataMember(Name = "recurring")]
        public bool RecurringPossible { get; set; }
        [DataMember(Name = "split")]
        public bool SplitPaymentPossible { get; set; }

        //[DisplayFormat(DataFormatString = "{0:0.00#}")]
        //public double RecurringFee { get {
        //    if (RecurringPossible)
        //    {
        //        switch (RecurringPeriod)
        //        {
        //            case "d":
        //                return Price/365;
        //            case "ww":
        //                return Price/52;
        //            case "m":
        //                return Price/12;
        //            default:
        //                return Price/12;
        //        }
        //    }
        //    return Price;
        //} }
        //public bool Recurring { get; set; }
        //public string RecurringPeriod { get; set; }
        //public string CartProductId { get; set; }

        //public bool Split { get; set; }
        //public int InstallementNumber { get; set; }
        //public double Installement { get { return InstallementNumber > 0 ? Price/InstallementNumber : Price/3; } }
    }
}
