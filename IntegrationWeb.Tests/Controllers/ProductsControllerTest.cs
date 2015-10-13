using System.Collections.Generic;
using System.Configuration;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntegrationWeb.Controllers;
using System.Web.Mvc;
using DomainModel;
using System.Web;
using IntegrationWeb.Models;
using System.Threading.Tasks;

namespace IntegrationWeb.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ProductsController _controller;

        [TestInitialize]
        public void Setup()
        {
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _controller = new ProductsController { ControllerContext = new Mocks.MockControllerContext() };
        }

        [TestMethod]
        public async Task WhenIndexActionIsCalled_IShouldGetViewWithModelFilled()
        {
            var viewResult = await _controller.Index() as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as ProductsViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(7, model.Products.Count);
        }

        [TestMethod]
        public async Task GivenCartIsANewCart_WhenCheckOutActionIsCalled_IShouldGetCheckoutView()
        {
            var context = new Mocks.MockControllerContext();
            context.HttpContext.Session["cart"] = new Cart();
            _controller = new ProductsController { ControllerContext = context };

            var viewResult = await _controller.CheckOut();
            Assert.IsNotNull(viewResult);
        }
    }
}
