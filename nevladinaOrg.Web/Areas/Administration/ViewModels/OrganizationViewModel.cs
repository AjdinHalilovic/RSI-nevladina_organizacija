using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class OrganizationViewModel
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageOrganizationTypeReq)),
         Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageOrganizationTypeReq))]
        public int OrganizationTypeId { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCountryReq))]
        public int CountryId { get; set; }

        public int? RegionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCityReq))]
        public int CityId { get; set; }

        public string Place { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageAddressReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageAddressReq))]
        public string Address { get; set; }

        public byte[] Logo { get; set; }

        public string AdditionalInformation { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }

        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

        public static implicit operator Organization(OrganizationViewModel model)
        {
            Organization organization = new Organization()
            {
                Id = model.Id,
                ParentId = model.ParentId,
                OrganizationTypeId = model.OrganizationTypeId,
                Name = model.Name,
                CountryId = model.CountryId,
                CityId = model.CityId,
                Place = model.Place,
                Address = model.Address,
                Logo = model.Logo,
                AdditionalInformation = model.AdditionalInformation,
                Active = model.Active,
                IsDeleted = model.IsDeleted
            };

            return organization;
        }

        public static implicit operator OrganizationViewModel(Organization model)
        {
            OrganizationViewModel organizationViewModelVM = new OrganizationViewModel()
            {
                Id = model.Id,
                ParentId = model.ParentId,
                OrganizationTypeId = model.OrganizationTypeId,
                Name = model.Name,
                CountryId = model.CountryId,
                CityId = model.CityId,
                Place = model.Place,
                Address = model.Address,
                Logo = model.Logo,
                AdditionalInformation = model.AdditionalInformation,
                Active = model.Active,
                IsDeleted = model.IsDeleted,
                SaveAndOptions = Enumerations.SaveAndOptions.SaveAndClose
            };

            return organizationViewModelVM;
        }
    }
}
