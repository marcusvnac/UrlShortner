using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShortherUrlCore.Business;
using ShortherUrlCore.Storage;
using ShortherUrlCore.Storage.Models;
using System;
using System.Threading.Tasks;

namespace ShortnerUrlCoreTests
{
    [TestClass]
    public class ShortnerUrlBSTest
    {
        private IShortnerUrlBS shortnerUrlBS;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockStorageManager = new Mock<IStorageManager>();
            mockStorageManager.Setup(
                x => x.Get(It.IsAny<string>()))
                .ReturnsAsync((string origUrl) => new ShortUrl(origUrl.GetHashCode().ToString("X8"), origUrl));

            shortnerUrlBS = new ShortnerUrlBS(mockStorageManager.Object);
        }

        [TestMethod]
        public async Task ShortnerGoldPath()
        {
            var originalUrl = "http://www.shopify.com/testyourstuffhere";

            var shortUrl = await shortnerUrlBS.Process(originalUrl);

            Assert.IsTrue(shortUrl.Length <= originalUrl.Length);
        }

        [TestMethod]
        public void ShortnerEmptyCurrentUrl()
        {
            Assert.ThrowsException<ArgumentException>(() => shortnerUrlBS.Process(""));
        }
    }
}
