using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;

namespace IntegrationWeb.Models
{
    public class PaymentMethodViewModel
    {
        public string ChosenId { get; set; }
        public IList<Wallet> Wallets { get; set; }
        /// <summary>
        /// Action to execute when not choosing any stored Alias
        /// </summary>
        public string ReturnAction { get; set; }
        /// <summary>
        /// Action to execute when choosing an Alias
        /// </summary>
        public string AliasAction { get; set; }
        /// <summary>
        /// Cannot choose to enter other payment method
        /// </summary>
        public bool CannotChooseOther { get; set; }
    }
}