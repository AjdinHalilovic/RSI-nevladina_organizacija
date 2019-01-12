using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas
{
    public class MemberLicenseViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string LicenceNumber { get; set; }
        public bool Permanent { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }

        public static implicit operator MemberLicense(MemberLicenseViewModel model)
        {
            MemberLicense license = new MemberLicense()
            {
                Id = model.Id,
                LicenceNumber=model.LicenceNumber,
                MemberId=model.MemberId,
                Permanent=model.Permanent,
                Active=model.Active
            };

            return license;
        }

        public static implicit operator MemberLicenseViewModel(MemberLicense model)
        {
            MemberLicenseViewModel license = new MemberLicenseViewModel()
            {
                Id = model.Id,
                LicenceNumber = model.LicenceNumber,
                MemberId = model.MemberId,
                Permanent = model.Permanent,
                Active = model.Active
            };

            return license;
        }
    }
}
