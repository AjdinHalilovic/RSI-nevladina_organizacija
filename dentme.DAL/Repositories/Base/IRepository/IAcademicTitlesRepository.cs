using Core.Entities.Base;
namespace DAL.Repositories.Base.IRepository
{
    public interface IAcademicTitlesRepository : IRepository<AcademicTitle, int>
    {
        bool GetExists(string name);
    }
}
