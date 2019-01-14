using Core.Entities.Base;
using Core.Entities.Base.DTO;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageNameReq)),
            StringLength(100,ErrorMessage =nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageStartDateReq))]
        public DateTime? DateFrom { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageEndDateReq))]
        public DateTime? DateTo { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessagePlaceReq)),
            StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessagePlaceLen100))]
        public string Place { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageCityReq))]
        public int CityId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCountryReq))]
        public int CountryId { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int InstitutionOrganizerId { get; set; }
        public int? OrganizationOrganizerId { get; set; }
        public List<EventItemDTO> EventItemsDTO { get; set; }
        public List<EventImage> EventImages { get; set; }
        public List<EventDocument> EventDocuments { get; set; }

        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

        public static implicit operator Event(EventViewModel model)
        {
            Event _event = new Event()
            {
                Id = model.Id,
                Name = model.Name,
                CityId=model.CityId,
                CountryId=model.CountryId,
                DateFrom=model.DateFrom,
                DateTo=model.DateTo,
                Place=model.Place,
                Description=model.Description,
                InstitutionOrganizerId=model.InstitutionOrganizerId,
                OrganizationOrganizerId=model.OrganizationOrganizerId,
                Latitude=model.Latitude,
                Longitude=model.Longitude
            };

            return _event;
        }
        public static implicit operator EventViewModel(Event model)
        {
            EventViewModel _event = new EventViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                CityId = model.CityId,
                CountryId = model.CountryId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                Place = model.Place,
                Description = model.Description,
                InstitutionOrganizerId = model.InstitutionOrganizerId,
                OrganizationOrganizerId = model.OrganizationOrganizerId,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            return _event;
        }

    }
}
