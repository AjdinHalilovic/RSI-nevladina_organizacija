using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class SponsorsDTORepository:Repository<SponsorDTO,int>,ISponsorsDTORepository
    {
        public SponsorsDTORepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<SponsorDTO>GetByEventId(int id)
        {
            return Context.EventSponsors.Where(x => !x.IsDeleted && x.EventId == id).Include(s => s.Sponsor).Include(t => t.SponsorType).OrderBy(x=>x.SponsorTypeId)
                .Select(x => new SponsorDTO
                {
                    Id = x.Id,
                    SponsorId=x.Sponsor.Id,
                    SponsorType = x.SponsorType.Name,
                    Name = x.Sponsor.Name,
                    Description = x.Sponsor.Description,
                    WebUrl = x.Sponsor.WebUrl,
                    LogoBase64 = x.Sponsor.Logo==null?string.Empty: string.Format("data:image/png|jpg;base64,{0}", Convert.ToBase64String(x.Sponsor.Logo))
                });
        }
        //public override IEnumerable<SponsorDTO> GetAll()
        //{
        //    return Context.EventSponsors.Where(x => !x.IsDeleted).Include(s => s.Sponsor).Include(t => t.SponsorType)
        //        .Select(x => new SponsorDTO
        //        {
        //            Id = x.Sponsor.Id,
        //            SponsorType =x.SponsorType.Name,
        //            Name =x.Sponsor.Name
        //            ,Description=x.Sponsor.Description,
        //            WebUrl =x.Sponsor.WebUrl,
        //            LogoBase64 = string.Format("data:image/png|jpg;base64,{0}", Convert.ToBase64String(x.Sponsor.Logo))
        //        });
        //}
    }
}
