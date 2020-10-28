using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class LicensePeriodViewModel
    {
        public int Id { get; set; }
        public int MemberLicenseId { get; set; }
        //[Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageLicenseTypeReq))] 
        public int LicenseTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator LicensePeriod(LicensePeriodViewModel model)
        {
            LicensePeriod licensePeriod = new LicensePeriod()
            {
                Id = model.Id,
                Active=model.Active,
                CreatedDate =model.CreatedDate,
                EndDate =model.EndDate,
                LicenseTypeId=model.LicenseTypeId,
                MemberLicenseId=model.MemberLicenseId
            };

            return licensePeriod;
        }

        public static implicit operator LicensePeriodViewModel(LicensePeriod model)
        {
            LicensePeriodViewModel licensePeriod = new LicensePeriodViewModel()
            {
                Id = model.Id,
                Active = model.Active,
                CreatedDate = model.CreatedDate,
                EndDate = model.EndDate,
                LicenseTypeId = model.LicenseTypeId,
                MemberLicenseId = model.MemberLicenseId
            };

            return licensePeriod;
        }
    }
}
