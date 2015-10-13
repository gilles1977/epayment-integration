using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Base;
using System.IO;

namespace DataAccess.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private const string _filePath = "Test.json";

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(_filePath);
        }

        [TestMethod]
        public void GivenICreateANewRepository_ANewOrderJSONFileShouldBeCreated()
        {
            var repository = new Repository<string>(_filePath, forceFileCreation: true);
            Assert.IsTrue(File.Exists(_filePath));
        }
    }
}
