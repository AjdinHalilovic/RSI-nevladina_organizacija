using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEmployeeStatusesRepository:IRepository<EmployeeStatus,int>
    {
        bool GetExists(string name);
    }
}
