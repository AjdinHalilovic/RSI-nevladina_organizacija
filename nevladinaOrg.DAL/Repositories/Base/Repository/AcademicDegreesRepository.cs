using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AcademicDegreesRepository : Repository<AcademicDegree, int>, IAcademicDegreesRepository
    {
        public AcademicDegreesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.AcademicDegrees.Any(x => x.Name == name && !x.IsDeleted);
        }
    }
}
