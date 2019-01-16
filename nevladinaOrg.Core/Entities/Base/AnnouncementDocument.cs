using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AnnouncementDocuments)]
    public class AnnouncementDocument : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid StreamId { get; set; }
        [Required,
         ForeignKey(nameof(Announcement))]
        public int AnnouncementId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Type { get; set; }
        public long Size { get; set; }
        [Required]
        public byte[] Document { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Announcement Announcement { get; set; }
    }
}
