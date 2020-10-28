using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventDocuments)]
    public class EventDocument : IEntity
    {
        [Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid StreamId { get; set; }
        [Required,
         ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        [Required]
        public byte[] Document { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
    }
}
