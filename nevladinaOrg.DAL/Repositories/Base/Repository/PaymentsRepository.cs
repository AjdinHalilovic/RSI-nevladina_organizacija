using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class PaymentsRepository:Repository<Payment,int>,IPaymentsRepository
    {
        public PaymentsRepository(NevladinaOrgContext context) : base(context) { }
        public int? Remove(int registrationId)
        {
            var payment = Context.Payments.FirstOrDefault(x=>x.RegistrationId==registrationId);
            if (payment != null)
            {
                Context.Payments.Remove(payment);
                return payment.Id;
            }
            return null;
        }
        public bool GetExists(int registrationId)
        {
            return Context.Payments.Any(x => !x.IsDeleted && x.RegistrationId == registrationId);
        }
    }
}
