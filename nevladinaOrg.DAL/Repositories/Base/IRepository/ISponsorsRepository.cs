using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ISponsorsRepository:IRepository<Sponsor,int>
    {
        bool GetExists(string name);
    }
}
