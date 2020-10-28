using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class LicensePeriodsRepository:Repository<LicensePeriod,int>,ILicensePeriodsRepository
    {
        public LicensePeriodsRepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<LicensePeriod> GetByLicenseId(int licenseId)
        {
            return Context.LicensePeriods.Where(x => x.MemberLicenseId == licenseId);
        }
        public void SetActiveLicesePeriodByLicenseId(int licensePeriodId,int licenseId)
        {
            var licensePeriods = Context.LicensePeriods.Where(x => !x.IsDeleted && x.Id!=licensePeriodId && x.MemberLicenseId == licenseId && x.Active == true).ToList();
            foreach (var period in licensePeriods)
            {
                period.Active = false;
            }
            Context.LicensePeriods.UpdateRange(licensePeriods);
        }
        public void DeactivateByLicenseId(int licenseId)
        {
            var licensePeriods = Context.LicensePeriods.Where(x => !x.IsDeleted && x.MemberLicenseId == licenseId && x.Active == true).ToList();
            foreach (var period in licensePeriods)
            {
                period.Active = false;
            }
            if (licensePeriods != null)
            {
                Context.LicensePeriods.UpdateRange(licensePeriods);
            }
        }
    }
}
