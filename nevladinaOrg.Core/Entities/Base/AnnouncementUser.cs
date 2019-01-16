using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AnnouncementUsers)]
    public class AnnouncementUser : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
         ForeignKey(nameof(Announcement))]
        public int AnnouncementId { get; set; }
        [Required,
         ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public DateTime? OccurredAt { get; set; }
        public bool Seen { get; set; }
        public bool IsDeleted { get; set; }

        public Announcement Announcement { get; set; }
        public User User { get; set; }
    }
}
