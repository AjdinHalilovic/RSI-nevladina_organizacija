using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class OrganizationDTORepository : Repository<OrganizationDTO, int>, IOrganizationsDTORepository
    {

        public OrganizationDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override IEnumerable<OrganizationDTO> GetAll()
        {
            return Context.Organizations.
                Include(x => x.Parent)
                .Include(x=>x.OrganizationType)
                .Include(x=>x.City)
                .Include(x=>x.Country)
                .Where(x => x.IsDeleted == false)
                .Select(x => new OrganizationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parent = x.Parent.Name,
                    OrganizationType=x.OrganizationType.Name,
                    Place = x.Place,
                    City=x.City.Name,
                    Country=x.Country.Name,
                    Address = x.Address,
                    AdditionalInformation = x.AdditionalInformation,
                    Active = x.Active
                });
        }

        public IEnumerable<OrganizationDTO> GetByParentId(int OrganizationParentId)
        {
            return Context.Organizations.
                Include(x => x.Parent).
                Where(x => x.IsDeleted == false && x.ParentId == OrganizationParentId).
                Select(x => new OrganizationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parent = x.Parent.Name,
                    Place = x.Place,
                    Address = x.Address,
                    AdditionalInformation = x.AdditionalInformation,
                    Active = x.Active
                });
        }
    }
}
