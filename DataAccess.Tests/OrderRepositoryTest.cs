using System;
using DataAccess.Base;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DataAccess.Tests
{
    [TestClass]
    public class OrderRepositoryTest
    {
        private const string _filePath = "Orders.json";

        [TestMethod]
        public void GivenANewRepositoryIsCreatedAndANewOrderIsCreated_WhenRepositorySave_TheOrderShouldBeSavedInFile()
        {
            var expectedContent = @"[{""Amount"":1235.12,""CardBrand"":null,""CardNo"":null,""Currency"":null,""OrderId"":""1234"",""PayId"":null,""Quantity"":0,""Status"":null,""TrxDate"":""\/Date(236556000000+0200)\/"",""UserId"":1}]";
            var repository = new Repository<Order>(_filePath, forceFileCreation: true);
            repository.Save(new Order() { OrderId = "1234", Amount = 1235.12, UserId = 1, TrxDate = new DateTime(1977,7,1) });

            var content = File.ReadAllText(_filePath);

            Assert.IsTrue(File.Exists(_filePath));
            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void GivenAnOrderIsSaved_RepositoryShouldReturnIt()
        {
            var repository = new Repository<Order>(_filePath, forceFileCreation: true);
            repository.Save(new Order() { OrderId = "1234", Amount = 1235.12, UserId = 1, TrxDate = new DateTime(1977, 7, 1) });

            var orders = repository.GetAll();

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count);
            Assert.AreEqual("1234", orders[0].OrderId);
        }
    }
}
