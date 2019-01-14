using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class RegionsDTORepository : Repository<RegionDTO, int>, IRegionsDTORepository
    {
        public RegionsDTORepository(NevladinaOrgContext context)
            : base(context)
        {
        }


        public override IEnumerable<RegionDTO> GetAll()
        {
            return Context.Regions.Include(x => x.Country).Where(x => x.IsDeleted == false).Select(x => new RegionDTO { Id = x.Id, Name = x.Name, Country = x.Country.Name });
        }
    }
}
