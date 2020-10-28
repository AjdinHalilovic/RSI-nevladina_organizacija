using DAL;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.Culture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace nevladinaOrg.Web.Controllers
{
    [Authorization]
    public class SystemLanguagesController : BaseController
    {
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly ICulture _culture;

        public SystemLanguagesController(IDataUnitOfWork dataUnitOfWork,
                                         ICulture culture,
                                         IBreadcrumb breadcrumb,
                                         IStringLocalizerFactory stringLocalizerFactory) : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _culture = culture;
        }

        [HttpGet]
        public IActionResult Change(string id)
        {
            _culture.Set(id);

            var loggedUser = HttpContext.GetLoggedUserData();

            if (loggedUser != null)
            {
                loggedUser.PersonUser.CultureName = id;
                HttpContext.SetLoggedUser(loggedUser);

                _dataUnitOfWork.BaseUow.UsersRepository.UpdateCultureName(loggedUser.User.Id, id);
                _dataUnitOfWork.BaseUow.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public JsonResult GetFlag()
        {
            var loggedUser = HttpContext.GetLoggedUserData();

            if (loggedUser != null && loggedUser.PersonUser.CultureName != null)
            {
                return Json(Configuration.SupportedSystemLanguages.FirstOrDefault(x => x.Name.ToLower() == loggedUser.PersonUser.CultureName.ToLower()).Icon);
            }

            return Json(Configuration.SupportedSystemLanguages.FirstOrDefault(x => x.Default).Icon);
        }
    }
}