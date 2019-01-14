using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository
{
    public interface IPaymentsRepository:IRepository<Payment,int>
    {
        int? Remove(int registrationId);
        bool GetExists(int registrationId);
    }
}
