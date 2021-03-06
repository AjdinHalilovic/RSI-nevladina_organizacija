﻿using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace DAL.Repositories.Base.Repository
{
    public class RolesRepository:Repository<Role,int>,IRolesRepository
    {
        public RolesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override IEnumerable<Role> GetAll()
        {
            return Context.Roles.Where(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
        }
        public bool GetExists(string name)
        {
            return Context.Roles.Any(x => x.Name == name && x.IsDeleted == false);
        }
        public IEnumerable<Role> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId)
        {
            return Context.UserRoles.Include(x => x.Role).Where(x => x.OrganizationInstitutionUserId == organizationInstitutionUserId).Select(x => x.Role);
        }
        public IEnumerable<Role> GetByInstitutionUserId(int institutionUserId)
        {
            return Context.UserRoles.Include(x => x.Role).Where(x => x.InstitutionUserId == institutionUserId).Select(x => x.Role);
        }

    }
}
