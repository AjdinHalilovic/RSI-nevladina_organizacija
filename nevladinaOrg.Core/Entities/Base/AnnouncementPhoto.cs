using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AnnouncementPhotos)]
    public class AnnouncementPhoto : IEntity
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
        [Required, StringLength(20)]
        public string Type { get; set; }
        public long Size { get; set; }
        [Required]
        public byte[] Photo { get; set; }
        public byte[] Thumbnail { get; set; }
        public bool IsDeleted { get; set; }
        public Announcement Announcement { get; set; }
    }
}
