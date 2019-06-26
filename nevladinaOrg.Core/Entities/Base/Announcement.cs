using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Announcements)]
    public class Announcement:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int? UserId { get; set; }

        public int? InstitutionId { get; set; }

        public int? OrganizationId { get; set; }
        [Required,
         ForeignKey(nameof(AnnouncementType))]
        public int? AnnouncementTypeId { get; set; }
        [Required,StringLength(80)]
        public string Title { get; set; }
        [StringLength(80)]
        public string Subtitle { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Active { get; set; } 
        public bool IsDeleted { get; set; }

        public ICollection<AnnouncementOrganization> AnnouncementsOrganizations { get; set; }
        public AnnouncementType AnnouncementType { get; set; }


    }
}
