using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AcademicTitlesRepository : Repository<AcademicTitle, int>, IAcademicTitlesRepository
    {
        public AcademicTitlesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.AcademicTitles.Any(x => x.Name == name && !x.IsDeleted);
        }
    }
}
