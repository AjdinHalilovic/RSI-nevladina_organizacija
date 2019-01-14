using Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Institutions)]
    public class Institution : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(ParentInstitution))]
        public int? ParentId { get; set; }

        [Required]
        [ForeignKey(nameof(InstitutionType))]
        public int InstitutionTypeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(100)]
        public string Address { get; set; }

        [Required, StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [Required, StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [DataType(DataType.Url)]
        public string WebsiteURL { get; set; }

        [DataType(DataType.Url)]
        public string SocialURL { get; set; }

        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public byte[] Logo { get; set; }

        public string AdditionalInformation { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }



        public City City { get; set; }
        public Country Country { get; set; }
        public InstitutionType InstitutionType { get; set; }
        public Institution ParentInstitution { get; set; }
    }
}
