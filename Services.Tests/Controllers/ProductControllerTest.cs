using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Controllers;
using Services.Tests.DataAccess;

namespace Services.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void GivenAProductRepository_WhenICallGetAllProducts_IShouldGetCorrectProducts()
        {
            var repository = new ProductRepositoryMock();
            var productsController = new ProductsController(repository);

            var products = productsController.GetAllProducts();
            
            Assert.IsNotNull(products);
            Assert.AreEqual(4, products.Count());
            Assert.IsNull(products.ElementAtOrDefault(3).Name);
        }
    }
}
