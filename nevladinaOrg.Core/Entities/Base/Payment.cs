using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Payments)]
    public class Payment:IEntity
    {
        [Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
        ForeignKey(nameof(EventRegistration))]
        public int RegistrationId { get; set; }
        public double Amount { get; set; }
        public DateTime DateOfPayment { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public EventRegistration EventRegistration { get; set; }
    }
}
