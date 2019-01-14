using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventItemEventTypes)]
    public class EventItemEventType:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
         ForeignKey(nameof(EventItem))]
        public int EventItemId { get; set; }
        [Required,
         ForeignKey(nameof(EventItemType))]
        public int EventItemTypeId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public EventItem EventItem { get; set; }
        public EventItemType EventItemType { get; set; }
    }
}
