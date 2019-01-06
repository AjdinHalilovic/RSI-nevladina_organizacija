using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventSponsors)]
    public class EventSponsor : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
         ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        [Required,
        ForeignKey(nameof(Sponsor))]
        public int SponsorId { get; set; }
        [Required,
        ForeignKey(nameof(SponsorType))]
        public int SponsorTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public Sponsor Sponsor { get; set; }
        public SponsorType SponsorType{ get; set; }

    }
}
