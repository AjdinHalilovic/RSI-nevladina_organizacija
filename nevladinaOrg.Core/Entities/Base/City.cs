using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Cities)]
    public class City : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Region))]
        public int? RegionId { get; set; }

        [Required,
         ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [Required,
         StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string PostalCode { get; set; }

        public bool IsDeleted { get; set; }


        public Region Region { get; set; }
        public Country Country { get; set; }
    }
}
