using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AnnouncementOrganizations)]
    public class AnnouncementOrganization :IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
         ForeignKey(nameof(Announcement))]
        public int AnnouncementId { get; set; }
        [Required,
         ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }
        public bool IsDeleted { get; set; }

        public Announcement Announcement { get; set; }
        public Organization Organization { get; set; }
    }
}
