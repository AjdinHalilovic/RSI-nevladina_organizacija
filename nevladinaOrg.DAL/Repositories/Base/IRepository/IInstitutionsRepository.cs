using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IInstitutionsRepository : IRepository<Institution, int>
    {
        bool GetExists(string name, int countryId, int cityId, int intitutionTypeId);
        IEnumerable<Institution> GetByInstitutionIds(int?[] institutionIds);
        IEnumerable<Institution> GetByInstitutionIds(int[] institutionIds);
        IEnumerable<Institution> GetByInstitutionIds(string institutionIds);
        //Institution GetByUserId(int userId);
    }
}