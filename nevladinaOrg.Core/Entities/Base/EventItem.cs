using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventItems)]
    public class EventItem:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
        ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? StartTime{ get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EndTime{ get; set; }
        public string ConferenceRoom{ get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
    }
}
