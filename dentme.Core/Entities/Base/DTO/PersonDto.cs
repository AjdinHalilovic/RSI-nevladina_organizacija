using System;

namespace Core.Entities.Base.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }

        public int? InstitutionUserId { get; set; }
        public int? OrganizationInstitutionUserId { get; set; }
        public int? InstitutionId { get; set; }
        

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ParentName { get; set; }

        public string FirstAndLastName => $"{FirstName} {LastName}";

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthString { get { return DateOfBirth.Value.ToShortDateString(); } }
        public bool Birthday => DateOfBirth?.Date == DateTime.Now.Date;
        public int Age => DateTime.Now.Year - DateOfBirth.Value.Year;

        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }




        public string CultureName { get; set; }

        public bool ChangePassword { get; set; }
        public int? BirthCountryId { get; set; }

        public int? BirthCityId { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string NationalIDNumber { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        public string Place { get; set; }
        public int? CitizenshipId { get; set; }
        public string UserEmail { get; set; }
    }
}
