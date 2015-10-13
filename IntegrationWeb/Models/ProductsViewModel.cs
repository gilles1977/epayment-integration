using System;
using System.Collections.Generic;
using DomainModel;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntegrationWeb.Models
{
    public class ProductsViewModel
    {
        public ProductsViewModel()
        {
            Subscription = new SubscriptionViewModel();
            PaymentViewModel = new ScheduledPaymentViewModel();
        }
        public IList<Product> Products { get; set; }
        public Cart Cart { get; set; }
        public User CurrentUser { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string ShaSign { get; set; }
        public string PaymentMethod { get; set; }
        [Display(Name = "Card Type"), Required]
        public string Brand { get; set; }
        [Display(Name = "Card Holder Name"), Required]
        public string CardHolderName { get; set; }
        [Display(Name = "Card Number"), RegularExpression(@"d{13,16}", ErrorMessage = "Credit card numbers must be a sequence of 13 to 16 digits"), Required]
        public string CardNumber { get; set; }
        [Display(Name = "Expiry Date"), RegularExpression(@"(0[1-9]|1[012])/\d{2}", ErrorMessage = "Expiry Date must match mm/yy pattern"), Required]
        public string ExpiryDate { get; set; }
        [DisplayName("CVC"), Range(100, 999, ErrorMessage = "CVC must be a numeric value from 100 to 999"), Required]
        public string Cvc { get; set; }
        [DisplayName("Store my Financial Data")]
        public bool StoreFinancialData { get; set; }
        public Wallet ChosenWallet { get; set; }
        public SubscriptionViewModel Subscription { get; set; }
        public ScheduledPaymentViewModel PaymentViewModel { get; set; }
    }
}