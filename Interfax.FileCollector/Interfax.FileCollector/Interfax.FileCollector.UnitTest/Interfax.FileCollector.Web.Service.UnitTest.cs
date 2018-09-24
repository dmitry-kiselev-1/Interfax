using System;
using System.Linq;
using System.Text;
using Interfax.FileCollector.UnitTest.FileCollectorServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interfax.FileCollector.UnitTest
{
    [TestClass]
    public class WebServiceTest
    {
        [TestMethod]
        public void GetFilesTest()
        {
            try
            {
                using (var service = new FileCollectorServiceClient())
                {
                    var result = service.GetFiles();
                }
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void UploadFileTest()
        {
            try
            {
                using (var service = new FileCollectorServiceClient())
                {
                    var result = service.UploadFile("Name " + Guid.NewGuid(), new DateTime(2000, 1, 1),
                        Encoding.UTF8.GetBytes("Новый тест"));
                }
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void DownloadFileTest()
        {
            try
            {
                using (var service = new FileCollectorServiceClient())
                {
                    var id = service.GetFiles().First().Id;
                    var result = service.DownloadFile(id);
                }
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

    }
}
