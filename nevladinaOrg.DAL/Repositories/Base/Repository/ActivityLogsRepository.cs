using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;

namespace DAL.Repositories.Base.Repository
{
    public class ActivityLogsRepository : Repository<ActivityLog, int>, IActivityLogsRepository
    {
        public ActivityLogsRepository(NevladinaOrgContext context) : base(context)
        {
        }
    }
}