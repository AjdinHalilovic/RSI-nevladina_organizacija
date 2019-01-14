using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Persons)]
    public class Person : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string ParentName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string NationalIDNumber { get; set; }


        [ForeignKey(nameof(BirthCountry))]
        public int? BirthCountryId { get; set; }
        [ForeignKey(nameof(BirthCity))]
        public int? BirthCityId { get; set; }

        [ForeignKey(nameof(Citizenship))]
        public int? CitizenshipId { get; set; }


        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }
        [ForeignKey(nameof(Region))]
        public int? RegionId { get; set; }
        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }

        public string Place { get; set; }
        public string Address { get; set; }

        [ForeignKey(nameof(ResidenceCity))]
        public int? ResidenceId { get; set; }
        public string ResidenceAddress { get; set; }

        public bool IsDeleted { get; set; }

        public City ResidenceCity { get; set; }
        public City BirthCity { get; set; }
        public City City { get; set; }
        public Region Region { get; set; }
        public Country BirthCountry { get; set; }
        public Country Country { get; set; }
        public Citizenship Citizenship { get; set; }
    }
}
