using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementTypesRepository : IRepository<AnnouncementType, int>
    {
        bool GetExists(string name);
    }
}
