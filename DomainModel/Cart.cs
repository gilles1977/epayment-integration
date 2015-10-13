using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
    /// <summary>
    /// Store products to be ordered.
    /// </summary>
    public class Cart
    {
        #region Members
        private readonly IList<OrderedProduct> _products; 
        #endregion

        #region Constructors
        public Cart()
        {
            _products = new List<OrderedProduct>();
        }

        public Cart(Product product)
        {
            _products = new List<OrderedProduct>(new OrderedProduct[] { new OrderedProduct(product) });
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Unique Id generated when ordering cart content.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Total price of product contained in the cart plus total subscriptions if any.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public double Total => Math.Round(_products.Where(p => !p.Recurring).Sum(p => p.Price), 3) + TotalSubscriptions;

        /// <summary>
        /// Total subscriptions for recurring products.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public double TotalSubscriptions => Math.Round(_products.Where(p => p.Recurring).Sum(p => p.RecurringFee), 3);

        /// <summary>
        /// Total scheduled payments.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public double TotalScheduledPayments => Math.Round(_products.Where(p => p.InstallementNumber > 0).Sum(p => p.Installement), 3);

        /// <summary>
        /// Number of items in the cart.
        /// </summary>
        public int ItemCount => _products.Count;

        /// <summary>
        /// List of products in the cart.
        /// </summary>
        public IList<OrderedProduct> Products => _products;

        /// <summary>
        /// List of products in the cart that can be subscribed to recurring payment.
        /// </summary>
        public IList<OrderedProduct> RecurringProducts => _products.Where(p => p.RecurringPossible).ToList();

        /// <summary>
        /// List of products in the cart that can be ordered with split payment facility.
        /// </summary>
        public IList<OrderedProduct> SplittedProducts => _products.Where(p => p.SplitPaymentPossible).ToList();

        /// <summary>
        /// True if cart contains any product that can be subscribed to recurring payment.
        /// </summary>
        public bool HasRecurringProducts => _products.Any(p => p.RecurringPossible);

        /// <summary>
        /// True if cart contains any product that has been chosen for recurring payment.
        /// </summary>
        public bool HasRecurringSubscription => _products.Any(p => p.Recurring);

        /// <summary>
        /// True if cart contains any product that can be ordered with split payment facility.
        /// </summary>
        public bool HasSplitProducts => _products.Any(p => p.SplitPaymentPossible);

        /// <summary>
        /// True if cart contains any product that has been chosen for split payment facility.
        /// </summary>
        public bool HasInstallements => _products.Any(p => p.InstallementNumber > 0);

        /// <summary>
        /// Returns product for the selected cart product id.
        /// </summary>
        /// <param name="cartProductId">The cart id of the selected product.</param>
        /// <returns></returns>
        public OrderedProduct this[string cartProductId] => _products.FirstOrDefault(p => p.CartProductId == cartProductId);

        #endregion

        #region Methods

        /// <summary>
        /// Add a product to the cart.
        /// </summary>
        /// <param name="orderedProduct">The product to add.</param>
        public void Add(OrderedProduct orderedProduct)
        {
            orderedProduct.CartProductId = Guid.NewGuid().ToString();
            _products.Add(orderedProduct);
        }

        /// <summary>
        /// Remove the cart with the selected product id.
        /// </summary>
        /// <param name="productId">The id of the product to remove.</param>
        /// <returns></returns>
        public bool Remove(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                return _products.Remove(product);
            }
            return false;
        }

        /// <summary>
        /// Clear products in the cart.
        /// </summary>
        public void Clear()
        {
            _products.Clear();
        }

        #endregion
    }
}
