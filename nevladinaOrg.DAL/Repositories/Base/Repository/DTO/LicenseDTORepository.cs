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
    public class LicenseDTORepository:Repository<LicenseDTO,int>,ILicenseDTORepository
    {
        public LicenseDTORepository(NevladinaOrgContext context):base(context)
        {
        }

        public IEnumerable<LicenseDTO> GetByMemberId(int Id)
        {
                return Context.LicensePeriods.Include(x => x.MemberLicense).Include(x => x.LicenseType).Where(x =>!x.IsDeleted && x.MemberLicense.MemberId == Id).
                    Select(x => new LicenseDTO
                    {
                        LicenseType=x.LicenseType.Name,
                        Active=x.Active,
                        CreatedDate=x.CreatedDate.Value.ToShortDateString(),
                        EndDate=x.EndDate.Value.ToShortDateString(),
                        Id=x.MemberLicense.Id,
                        LicenseNumber=x.MemberLicense.LicenceNumber,
                        LicensePeriodId=x.Id
                    }).OrderByDescending(x=> x.Active).ThenByDescending(x=>x.CreatedDate);
        }
    }
}
