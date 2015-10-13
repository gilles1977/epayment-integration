using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class OrderedProduct : Product
    {
        public OrderedProduct(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Price = product.Price;
            RecurringPossible = product.RecurringPossible;
            SplitPaymentPossible = product.SplitPaymentPossible;
        }

        [DisplayFormat(DataFormatString = "{0:0.00#}")]
        public double RecurringFee
        {
            get
            {
                if (RecurringPossible)
                {
                    switch (RecurringPeriod)
                    {
                        case "d":
                            return Price / 365;
                        case "ww":
                            return Price / 52;
                        case "m":
                            return Price / 12;
                        default:
                            return Price / 12;
                    }
                }
                return Price;
            }
        }
        public bool Recurring { get; set; }
        public string RecurringPeriod { get; set; }
        public string CartProductId { get; set; }

        public bool Split { get; set; }
        public int InstallementNumber { get; set; }
        public double Installement => InstallementNumber > 0 ? Price / InstallementNumber : Price / 3;
    }
}
