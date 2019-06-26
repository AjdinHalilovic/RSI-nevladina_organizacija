using DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using nevladinaOrg.Web.Areas.Administration.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
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
        }
    }
}
