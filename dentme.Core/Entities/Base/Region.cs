using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Regions)]
    public class Region : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required,
         ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public Country Country { get; set; }

        public Region(string name, int countryId)
        {
            CountryId = countryId;
            Name = name;
        }
    }
}
