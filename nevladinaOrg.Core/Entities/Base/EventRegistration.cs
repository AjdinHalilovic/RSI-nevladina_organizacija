using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventRegistrations)]
    public class EventRegistration:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
        ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        [Required,
        ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public bool Paid { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public User User { get; set; }

    }
}
