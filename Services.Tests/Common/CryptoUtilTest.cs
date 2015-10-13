using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.Tests.Common
{
    [TestClass]
    public class CryptoUtilTest
    {
        [TestMethod]
        public void GivenAString_WhenICallGetSha1SignatureOnTheString_IShouldGetTheRightHashedString()
        {
            var expected = "A235A073C026CCBD04C18CB82A6D9591EB6E719B";
            var str =
                "AMOUNT=546azertyuiop123456CN=Gilles Coenraetsazertyuiop123456CURRENCY=EURazertyuiop123456LANGUAGE=FRazertyuiop123456ORDERID=D627EEBAazertyuiop123456PSPID=CGIDEVazertyuiop123456";
            var actual = CryptoUtils.GetSha1Signature(str);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GivenAString_WhenIEncryptTheStringAndDecryptTheCryptedResult_IShouldGetTheOriginalString()
        {
            var expected = "4111111111111111";
            var crypted = CryptoUtils.EncryptAes(expected, "azertyuiop123456", "16151413121110987654321");
            var decrypted = CryptoUtils.DecryptAes(crypted, "azertyuiop123456", "16151413121110987654321");
            Assert.AreEqual(expected, decrypted);
        }
    }
}
