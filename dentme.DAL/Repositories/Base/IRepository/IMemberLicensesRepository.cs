using Core.Entities.Base;
namespace DAL.Repositories.Base.IRepository
{
    public interface IMemberLicensesRepository:IRepository<MemberLicense,int>
    {
        MemberLicense GetByMemberId(int Id);
        int Remove(int memberId);
        void SetActiveLicenseByMemberId(int licenseId, int memberId);
    }
}
