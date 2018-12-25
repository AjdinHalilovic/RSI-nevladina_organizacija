using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace nevladinaOrg.Web.Helpers.SelectListHelper
{
    public class SelectListHelper : ISelectListHelper
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly Localizer _localizer;
        #endregion

        public SelectListHelper(IDataUnitOfWork dataUnitOfWork, Localizer localizer)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _localizer = localizer;
        }

    }
}