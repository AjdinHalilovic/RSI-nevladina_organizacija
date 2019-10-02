using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class InstitutionsRepository : Repository<Institution, int>, IInstitutionsRepository
    {
        public InstitutionsRepository(NevladinaOrgContext context) : base(context)
        {
        }
        public IEnumerable<Institution> GetByInstitutionIds(int?[] institutionIds)
        {
            if ((institutionIds?.Length ?? 0) == 0)
                return null;

            return Context.Institutions.Where(x => institutionIds.Contains(x.Id));
        }

        public IEnumerable<Institution> GetByInstitutionIds(int[] institutionIds)
        {
            if (institutionIds.Length == 0)
                return null;

            return Context.Institutions.Where(x => institutionIds.Contains(x.Id));
        }

        public IEnumerable<Institution> GetByInstitutionIds(string institutionIds)
        {
            if (!string.IsNullOrEmpty(institutionIds))
                return null;

            int[] _institutionIds = Array.ConvertAll(institutionIds.Split(','), Convert.ToInt32);

            return Context.Institutions.Where(x => _institutionIds.Contains(x.Id));
        }

        public bool GetExists(string name, int countryId, int cityId, int intitutionTypeId)
        {
            return Context.Institutions.Any(x => x.Name == name && x.CityId == cityId && x.InstitutionTypeId == intitutionTypeId && x.IsDeleted == false);
        }
        //public Institution GetByUserId(int userId)
        //{
        //    var institutionId = Context.InstitutionUsers.Where(iu => !iu.IsDeleted && iu.UserId == userId && iu.Active).Select(iu => iu.InstitutionId).FirstOrDefault();

        //    return institutionId == default(int) ? null : Context.Institutions.Find(institutionId);
        //}
        //Potrebno korigovati
    }
}