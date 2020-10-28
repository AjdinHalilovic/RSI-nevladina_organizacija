using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface ILicensePeriodsRepository:IRepository<LicensePeriod,int>
    {
        IEnumerable<LicensePeriod> GetByLicenseId(int licenseId);
        void SetActiveLicesePeriodByLicenseId(int licensePeriodId, int licenseId);
        void DeactivateByLicenseId(int licenseId);
    }
}
