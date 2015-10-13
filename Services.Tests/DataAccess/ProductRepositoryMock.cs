using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Interface;
using DomainModel;

namespace Services.Tests.DataAccess
{
    public class ProductRepositoryMock : IRepository<Product>
    {
        private readonly IList<Product> _data;

        public ProductRepositoryMock()
        {
            var product1 = new Product()
                {
                    Name = "Test1",
                    Price = 2.1,
                    RecurringPossible = true,
                    SplitPaymentPossible = false
                };
            var product2 = new Product()
            {
                Name = "Test2",
                Price = 0,
                RecurringPossible = true,
                SplitPaymentPossible = false
            };
            var product3 = new Product()
            {
                Name = string.Empty,
                Price = 2.1,
                SplitPaymentPossible = false
            };
            var product4 = new Product();

            this._data = new List<Product>(new Product[] {product1, product2, product3, product4});
        }

        public IList<Product> GetAll()
        {
            return _data;
        }

        public void Save(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
