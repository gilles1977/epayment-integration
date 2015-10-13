using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Utils;
using Services.Controllers;
using Services.Tests.DataAccess;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace Services.Tests.Controllers
{
    [TestClass]
    public class UserAuthControllerTest
    {
        [TestMethod]
        public void GivenAUserNameAndAPassword_WhenICallPostValidate_ResponseShouldReturnSuccessCode()
        {
            var controller = new UserAuthController(new UserRepositoryMock());
            var httpConfig = new HttpConfiguration();

            controller.Configuration = httpConfig;
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfig;

            var response = controller.PostValidate(new UserModel() {UserName = "jdo", Password = CryptoUtils.HashPassword("1234") });
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GivenAUserName_WhenICallGetUser_IShouldGetMatchingUser()
        {
            var controller = new UserAuthController(new UserRepositoryMock());
            var user = controller.GetUser("jdo");
            Assert.IsNotNull(user);
            Assert.AreEqual("John Doe", user.FullName);
        }
    }
}
