using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventUsers)]
    public class EventUser:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        [Required,ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public DateTime? OccurredAt { get; set; }
        public bool Seen { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public User User { get; set; }

    }
}
