using Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Organizations)]
    public class Organization : IEntity
    {
        [Key, 
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Institution))]
        public int? InstitutionId { get; set; }

        [ForeignKey(nameof(Parent))]
        public int? ParentId { get; set; }

        /* DB - Path enumeration */
        public string ParentPath { get; set; }

        [Required]
        [ForeignKey(nameof(OrganizationType))]
        public int OrganizationTypeId { get; set; }

        [Required,
         StringLength(100)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public string Place { get; set; }

        [Required]
        public string Address { get; set; }

        public byte[] Logo { get; set; }

        public string AdditionalInformation { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }




        public Organization Parent { get; set; }
        public Institution Institution { get; set; }
        public OrganizationType OrganizationType { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
    }
}
