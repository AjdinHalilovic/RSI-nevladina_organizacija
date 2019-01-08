using Core.Entities.Base;
using System;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class PaymentViewModel
    {
        public EventRegistration eventRegistration { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime DateOfPayment { get; set; }
        public double Amount { get; set; }

        public static implicit operator Payment(PaymentViewModel model)
        {
            Payment payment = new Payment()
            {
                DateOfPayment=model.DateOfPayment,
                Amount=model.Amount
            };

            return payment;
        }
        public static implicit operator PaymentViewModel(Payment model)
        {
            PaymentViewModel payment = new PaymentViewModel()
            {
                DateOfPayment = model.DateOfPayment,
                Amount = model.Amount
            };

            return payment;
        }
    }
}
