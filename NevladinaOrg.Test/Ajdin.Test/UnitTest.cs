using DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using nevladinaOrg.Web.Areas.Administration.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;

namespace NevladinaOrg.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            Mock<IDataUnitOfWork> _dataUnitOfWork = new Mock<IDataUnitOfWork>();
            Mock<Localizer> Localizer = new Mock<Localizer>();
            Mock<ILogger> Logger = new Mock<ILogger>();
            Mock<Breadcrumb> BreadCrumb = new Mock<Breadcrumb>();
            Mock<IStringLocalizerFactory> LocalizerFactory = new Mock<IStringLocalizerFactory>();
            Mock<IHostingEnvironment> hosting = new Mock<IHostingEnvironment>();
            Mock<IImageHelper> imageHelper = new Mock<IImageHelper>();

            EventsController eventsController = new EventsController(_dataUnitOfWork.Object, hosting.Object, Localizer.Object, Logger.Object, imageHelper.Object, BreadCrumb.Object, LocalizerFactory.Object);


        }

        [TestMethod]
        public void GetRandomString()
        {
            var actual = PasswordGenerator.GenerateRandomAlphanumericString(4);

            Assert.AreNotEqual(string.Empty, actual);
        }
    }
}
