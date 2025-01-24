using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShortherUrlCore.Business;
using ShortherUrlCore.Models;
using ShortherUrlCore.Storage;
using System;
using System.Threading.Tasks;

namespace ShortnerUrlCoreTests
{
    [TestClass]
    public class ShortnerUrlBSTest
    {
        public required ShortnerUrlBS shortnerUrlBS;
        public required Mock<IStorageManager> mockStorageManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockStorageManager = new Mock<IStorageManager>();
            shortnerUrlBS = new ShortnerUrlBS(mockStorageManager.Object) ?? throw new Exception("ShortnerUrlBS cannot be null");
        }

        [TestMethod]
        public async Task ShortnerGoldPath()
        {
            var originalUrl = "http://www.shopify.com/testyourstuffhere";
            var expectedShortUrl = "5B452990C0";
            mockStorageManager.Reset();
            mockStorageManager.Setup(
               x => x.Get(It.IsAny<string>()))
               .ReturnsAsync((string origUrl) => new ShortUrl { ShortnedUrl = expectedShortUrl, OriginalUrl = originalUrl });

            var shortUrl = await shortnerUrlBS.Process(originalUrl);

            Assert.IsTrue(shortUrl.Length <= originalUrl.Length);
            Assert.AreEqual(expectedShortUrl, shortUrl);
        }

        [TestMethod]
        public void ShortnerEmptyCurrentUrl()
        {
            Assert.ThrowsExceptionAsync<ArgumentException>(() => shortnerUrlBS.Process(""));
        }

        [TestMethod]
        public async Task ShortnerUsePreviousStoredValue()
        {
            var originalUrl = "http://www.shopify.com/testyourstuffhere";
            var expectedShortUrl = "5B452990C0";
            mockStorageManager.Reset();

            ShortUrl? res = null;
            mockStorageManager.Setup(
               x => x.Get(It.IsAny<string>()))
               .Returns(Task.FromResult(res));

            mockStorageManager.Setup(
               x => x.Insert(It.IsAny<ShortUrl>()))
               .Returns(Task.CompletedTask);

            // call first time
            var shortUrl = await shortnerUrlBS.Process(originalUrl);

            mockStorageManager.Setup(
               x => x.Get(It.IsAny<string>()))
               .ReturnsAsync((string origUrl) => new ShortUrl { ShortnedUrl = expectedShortUrl, OriginalUrl = originalUrl });
            
            // call second time
            shortUrl = await shortnerUrlBS.Process(originalUrl);

            Assert.IsTrue(shortUrl.Length <= originalUrl.Length);
            Assert.AreEqual(expectedShortUrl, shortUrl);

            mockStorageManager.Verify(
                x => x.Get(It.IsAny<string>()),
                Times.Exactly(2));

            mockStorageManager.Verify(
                x => x.Insert(It.IsAny<ShortUrl>()),
                Times.Once());
        }
    }
}
