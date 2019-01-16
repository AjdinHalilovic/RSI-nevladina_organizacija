using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class InstitutionViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        [Range(1,int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageIntitutionTypeReq))]
        public int InstitutionTypeId { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageAddressReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageAddressLen100))]
        public string Address { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageContactPhoneReq)),
        StringLength(20, ErrorMessage = nameof(Localizer.ErrorMessageContactPhoneLen20))]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageEmailReq)),
        StringLength(50, ErrorMessage = nameof(Localizer.ErrorMessageEmailLen50))]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [DataType(DataType.Url)]
        public string WebsiteURL { get; set; }

        [DataType(DataType.Url)]
        public string SocialURL { get; set; }

        [Range(1,int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCountryReq))]
        public int CountryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCityReq))]
        public int CityId { get; set; }
        public byte[] Logo { get; set; }
        public IFormFile File { get; set; }

        public string AdditionalInformation { get; set; }
        public bool Active { get; set; }



        public static implicit operator Institution(InstitutionViewModel model)
        {
            Institution institution = new Institution()
            {
                Id = model.Id,
                ParentId = model.ParentId,
                InstitutionTypeId = model.InstitutionTypeId,
                Name = model.Name,
                Address = model.Address,
                ContactPhone = model.ContactPhone,
                ContactEmail=model.ContactEmail,
                WebsiteURL=model.WebsiteURL,
                SocialURL=model.SocialURL,
                CountryId=model.CountryId,
                CityId=model.CityId,
                Logo=model.Logo,
                AdditionalInformation=model.AdditionalInformation,
                Active=model.Active
            };

            return institution;
        }
        public static implicit operator InstitutionViewModel(Institution model)
        {
            InstitutionViewModel institution = new InstitutionViewModel()
            {
                Id = model.Id,
                ParentId = model.ParentId,
                InstitutionTypeId = model.InstitutionTypeId,
                Name = model.Name,
                Address = model.Address,
                ContactPhone = model.ContactPhone,
                ContactEmail = model.ContactEmail,
                WebsiteURL = model.WebsiteURL,
                SocialURL = model.SocialURL,
                CountryId = model.CountryId,
                CityId = model.CityId,
                Logo = model.Logo,
                AdditionalInformation = model.AdditionalInformation,
                Active = model.Active
            };

            return institution;
        }
    }
}
