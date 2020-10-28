using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class MemberLicensesRepository:Repository<MemberLicense,int>,IMemberLicensesRepository
    {
        public MemberLicensesRepository(NevladinaOrgContext context) : base(context) { }
        public MemberLicense GetByMemberId(int Id)
        {
            return Context.MemberLicenses.Where(x =>!x.IsDeleted && x.MemberId == Id).OrderByDescending(x=>x.CreatedDate).FirstOrDefault();
        }
        public int Remove(int memberId)
        {
            var license = Context.MemberLicenses.FirstOrDefault(x => x.MemberId == memberId);
            Context.MemberLicenses.Remove(license);
            return license.Id;
        }
        public void SetActiveLicenseByMemberId(int licenseId,int memberId)
        {
            var licenses=Context.MemberLicenses.Where(x => !x.IsDeleted && x.MemberId == memberId && x.Id != licenseId && x.Active==true).ToList();
            foreach (var licese in licenses)
            {
                licese.Active = false;

                var licensePeriods = Context.LicensePeriods.Where(x => !x.IsDeleted && x.MemberLicenseId == licese.Id && x.Active==true).ToList();
                foreach (var period in licensePeriods)
                {
                    period.Active = false;
                }
                Context.LicensePeriods.UpdateRange(licensePeriods);
            }
            Context.MemberLicenses.UpdateRange(licenses);
        }
    }
}
