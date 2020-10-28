using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Events)]
    public class Event:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
        public string Place { get; set; }
        [Required,
         ForeignKey(nameof(City))]
        public int CityId { get; set; }
        [Required,
         ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [ForeignKey(nameof(Organization))]
        public int? OrganizationOrganizerId { get; set; }
        [Required,
         ForeignKey(nameof(Institution))]
        public int InstitutionOrganizerId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public City City { get; set; }
        public Country Country { get; set; }
        public Institution Institution { get; set; }
        public Organization Organization { get; set; }

    }
}
