using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class CitiesDTORepository : Repository<CityDTO, int>, ICitiesDTORepository
    {
        public CitiesDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override CityDTO GetById(int id)
        {
            return Context.Cities.Include(x => x.Region).Include(x => x.Country).Select(x => new CityDTO { Id = x.Id, City = x.Name, PostalCode = x.PostalCode, Region = x.Region.Name, Country = x.Country.Name }).FirstOrDefault(x => x.Id == id);
        }

        public override IEnumerable<CityDTO> GetAll()
        {
            return Context.Cities.Where(x=>x.IsDeleted == false).Include(x => x.Region).Include(x => x.Country).Select(x => new CityDTO { Id = x.Id, City = x.Name, PostalCode = x.PostalCode, Region = x.Region.Name, Country = x.Country.Name });
        }
    }
}
