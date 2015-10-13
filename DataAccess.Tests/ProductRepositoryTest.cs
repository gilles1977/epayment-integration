using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel;
using DataAccess.Base;

namespace DataAccess.Tests
{
    [TestClass]
    public class ProductRepositoryTest
    {
        [TestMethod]
        public void GivenAnExistingProductFile_WhenRepositoryIsCreated_ProductsShouldBeLoaded()
        {
            var repository = new Repository<Product>(@"Products.json");
            var products = repository.GetAll();
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count == 7);
        }
    }
}
