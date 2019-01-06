using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventsRepository : IRepository<Event, int>
    {
        bool GetExists(string name);
    }
}
