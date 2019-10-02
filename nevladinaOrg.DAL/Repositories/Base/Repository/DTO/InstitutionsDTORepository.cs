using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class InstitutionsDTORepository : Repository<InstitutionDTO, int>, IInstitutionsDTORepository
    {
        public InstitutionsDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override IEnumerable<InstitutionDTO> GetAll()
        {
            return Context.Institutions.Where(x => x.IsDeleted == false).Include(x => x.ParentInstitution).Include(x=>x.City).Include(x=>x.Country)
                .Include(x=>x.InstitutionType)
                .Select(x => new InstitutionDTO
                {
                    Id = x.Id,
                    Parent = x.ParentInstitution.Name,
                    InstitutionType = x.InstitutionType.Name,
                    Name = x.Name,
                    Address = x.Address,
                    ContactPhone = x.ContactPhone,
                    ContactEmail = x.ContactEmail,
                    WebsiteURL = x.WebsiteURL,
                    SocialURL = x.SocialURL,
                    Country = x.Country.Name,
                    City = x.City.Name,
                    Logo = x.Logo,
                    AdditionalInformation = x.AdditionalInformation,
                    Active = x.Active
                });
        }

    }
}
