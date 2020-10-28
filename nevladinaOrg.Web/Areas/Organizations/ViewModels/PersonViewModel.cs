using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageFirstNameReq))]
        public string FirstName { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageLastNameReq))]
        public string LastName { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageParentNameReq))]
        public string ParentName { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageDateOfBirthReq))]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageGenderReq))]
        public string Gender { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string NationalIDNumber { get; set; }
        public int? BirthCountryId { get; set; }
        public int? BirthCityId { get; set; }
        public int? CitizenshipId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageCountryReq))]
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageCityReq))]
        public int? CityId { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public int? ResidenceId { get; set; }
        public string ResidenceAddress { get; set; }

        public bool IsDeleted { get; set; }

        public static implicit operator Person(PersonViewModel model)
        {
            Person person = new Person()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ParentName = model.ParentName,
                BirthCityId = model.BirthCityId,
                BirthCountryId = model.BirthCountryId,
                DateOfBirth = model.DateOfBirth,
                Place = model.Place,
                Address = model.Address,
                RegionId = model.RegionId,
                ResidenceAddress = model.ResidenceAddress,
                ResidenceId = model.ResidenceId,
                CitizenshipId = model.CitizenshipId,
                CityId = model.CityId,
                CountryId = model.CountryId,
                Gender = model.Gender,
                NationalIDNumber = model.NationalIDNumber,
                SocialSecurityNumber = model.SocialSecurityNumber,
            };

            return person;
        }

        public static implicit operator PersonViewModel(Person model)
        {
            PersonViewModel person = new PersonViewModel()
            {
                Id = model.Id,
                FirstName=model.FirstName,
                LastName=model.LastName,
                ParentName=model.ParentName,
                BirthCityId=model.BirthCityId,
                BirthCountryId=model.BirthCountryId,
                DateOfBirth=model.DateOfBirth,
                Place=model.Place,
                Address=model.Address,
                RegionId=model.RegionId,
                ResidenceAddress=model.ResidenceAddress,
                ResidenceId=model.ResidenceId,
                CitizenshipId=model.CitizenshipId,
                CityId=model.CityId,
                CountryId=model.CountryId,
                Gender=model.Gender,
                NationalIDNumber=model.NationalIDNumber,
                SocialSecurityNumber=model.SocialSecurityNumber,
            };

            return person;
        }
    }
}
