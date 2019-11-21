using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace nevladinaOrg.Web.Helpers.SelectListHelper
{
    public class SelectListHelper : ISelectListHelper
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly Localizer _localizer;
        #endregion

        public SelectListHelper(IDataUnitOfWork dataUnitOfWork, Localizer localizer)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _localizer = localizer;
        }

        #region Announcements
        public List<SelectListItem> AnnouncementTypes(bool includeEmpty = true)
        {
            List<AnnouncementType> list = _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetAll().ToList();

            var announcementTypes = new List<SelectListItem>();

            if (includeEmpty)
                announcementTypes.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectAnnouncementTypes });

            announcementTypes.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return announcementTypes;
        }
        #endregion

        #region Cities
        public List<SelectListItem> Cities(bool includeEmpty = false)
        {
            List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetAll().ToList();

            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCity });

            cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return cities;
        }
        public List<SelectListItem> CitiesBase(bool includeEmpty = false)
        {
            List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetAll().ToList();

            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCity });

            cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return cities;
        }

        public List<SelectListItem> CitiesByCountryID(int? countryId, bool includeEmpty = false)
        {
            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCity });

            if (countryId.HasValue)
            {
                List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetByCountryId(countryId.Value).ToList();

                cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            }
            return cities;
        }
        public List<SelectListItem> CitiesBaseByCountryID(int? countryId, bool includeEmpty = false)
        {
            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCity });

            if (countryId.HasValue)
            {
                List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetByCountryId(countryId.Value).ToList();

                cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            }
            return cities;
        }

        public List<SelectListItem> CitiesByRegionID(int regionId, bool includeEmpty = false)
        {
            List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetByRegionId(regionId).ToList();

            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCity });

            cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return cities;
        }

        #endregion

        #region Countries

        public List<SelectListItem> Countries(bool includeEmpty = false)
        {
            List<Country> list = _dataUnitOfWork.BaseUow.CountriesRepository.GetAll().ToList();

            var countries = new List<SelectListItem>();

            if (includeEmpty)
                countries.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCountry });

            countries.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return countries;
        }
        public List<SelectListItem> CountriesBase(bool includeEmpty = false)
        {
            List<Country> list = _dataUnitOfWork.BaseUow.CountriesRepository.GetAll().ToList();

            var countries = new List<SelectListItem>();

            if (includeEmpty)
                countries.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCountry });

            countries.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return countries;
        }

        #endregion

        #region Regions
        public List<SelectListItem> Regions(bool includeEmpty = false)
        {
            List<Region> list = _dataUnitOfWork.BaseUow.RegionsRepository.GetAll().ToList();

            var regions = new List<SelectListItem>();

            if (includeEmpty)
                regions.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectRegion });

            regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return regions;
        }

        public List<SelectListItem> RegionsByCountryID(int? countryId, bool includeEmpty = false)
        {
            var regions = new List<SelectListItem>();

            if (includeEmpty)
                regions.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectRegion });

            if (countryId.HasValue)
            {
                List<Region> list = _dataUnitOfWork.BaseUow.RegionsRepository.GetByCountryId(countryId.Value).ToList();

                regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            }
            return regions;
        }
        #endregion

        #region Citizenships

        public List<SelectListItem> Citizenships(bool includeEmpty = true)
        {
            List<Citizenship> list = _dataUnitOfWork.BaseUow.CitizenshipsRepository.GetAll().ToList();

            var regions = new List<SelectListItem>();

            if (includeEmpty)
                regions.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectCitizenship });

            regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return regions;
        }

        #endregion

        #region LicenseTypes

        public List<SelectListItem> LicenseTypes(bool includeEmpty = true)
        {
            List<LicenseType> list = _dataUnitOfWork.BaseUow.LicenseTypesRepository.GetAll().ToList();

            var regions = new List<SelectListItem>();

            if (includeEmpty)
                regions.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectLicenceType });

            regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return regions;
        }

        #endregion

        #region Sponsors

        public List<SelectListItem> Sponsors(bool includeEmpty = true)
        {
            List<Sponsor> list = _dataUnitOfWork.BaseUow.SponsorsRepository.GetAll().ToList();

            var sponsors = new List<SelectListItem>();

            if (includeEmpty)
                sponsors.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectSponsor });

            sponsors.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return sponsors;
        }
        public List<SelectListItem> SponsorTypes(bool includeEmpty = true)
        {
            List<SponsorType> list = _dataUnitOfWork.BaseUow.SponsorTypesRepository.GetAll().ToList();

            var sponsorTypes = new List<SelectListItem>();

            if (includeEmpty)
                sponsorTypes.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectSponsorType });

            sponsorTypes.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return sponsorTypes;
        }

        #endregion

        #region Events

        public List<SelectListItem> EventItemTypes(bool includeEmpty = true)
        {
            List<EventItemType> list = _dataUnitOfWork.BaseUow.EventItemTypesRepository.GetAll().ToList();

            var eventItemTypes = new List<SelectListItem>();

            if (includeEmpty)
                eventItemTypes.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectEventItemTypes });

            eventItemTypes.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return eventItemTypes;
        }
        public List<SelectListItem> Lecturers(bool includeEmpty = true)
        {
            List<Lecturer> list = _dataUnitOfWork.BaseUow.LecturersRepository.GetAll().ToList();

            var lecturers = new List<SelectListItem>();

            if (includeEmpty)
                lecturers.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectEventItemTypes });

            lecturers.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FirstName + ' ' + x.LastName }));

            return lecturers;
        }



        #endregion


        #region Users

        public List<SelectListItem> Persons(bool includeEmpty = true)
        {
            List<Person> list = _dataUnitOfWork.BaseUow.PersonsRepository.GetAll().ToList();

            var persons = new List<SelectListItem>();

            if (includeEmpty)
                persons.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectPatient });

            persons.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FirstName + " " + x.LastName }));

            return persons;
        }


        #endregion

        #region MartialStatuses

        public List<SelectListItem> MartialStatuses(bool includeEmpty = false)
        {
            List<MaritalStatus> list = _dataUnitOfWork.BaseUow.MartialStatusesRepository.GetAll().ToList();

            var maritalStatus = new List<SelectListItem>();

            if (includeEmpty)
                maritalStatus.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectMartialStatus });

            maritalStatus.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return maritalStatus;
        }

        #endregion

        #region AcademicDegrees

        public List<SelectListItem> AcademicDegrees(bool includeEmpty = false)
        {
            List<AcademicDegree> list = _dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetAll().ToList();

            var academicDegrees = new List<SelectListItem>();

            if (includeEmpty)
                academicDegrees.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectAcademicDegree });

            academicDegrees.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return academicDegrees;
        }

        #endregion

        #region AcademicTitles

        public List<SelectListItem> AcademicTitles(bool includeEmpty = false)
        {
            List<AcademicTitle> list = _dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetAll().ToList();

            var academicTitles = new List<SelectListItem>();

            if (includeEmpty)
                academicTitles.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectAcademicTitle });

            academicTitles.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return academicTitles;
        }

        #endregion

        #region Roles
        public List<SelectListItem> Roles(bool includeEmpty = false)
        {
            List<Role> list = _dataUnitOfWork.BaseUow.RolesRepository.GetAll().ToList();

            var roles = new List<SelectListItem>();

            if (includeEmpty)
                roles.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectRoles });

            roles.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return roles;
        }

        #endregion

        #region Organizations
        public List<SelectListItem> Organizations(bool includeEmpty = true)
        {
            List<Organization> list = _dataUnitOfWork.BaseUow.OrganizationRepository.GetAll().ToList();

            var organizations = new List<SelectListItem>();

            if (includeEmpty)
                organizations.Add(new SelectListItem { Value = string.Empty, Text = _localizer.ChooseOrganization });

            organizations.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return organizations;
        }

        public List<SelectListItem> OrganizationTypes(bool includeEmpty = true)
        {
            List<OrganizationType> list = _dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetAll().ToList();

            var organizationTypes = new List<SelectListItem>();

            if (includeEmpty)
                organizationTypes.Add(new SelectListItem { Value = string.Empty, Text = _localizer.ChooseOrganizationType });

            organizationTypes.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return organizationTypes;
        }
        #endregion


        #region EmployementStatuses

        public List<SelectListItem> EmployementStatuses(bool includeEmpty = false)
        {
            List<EmployeeStatus> list = _dataUnitOfWork.BaseUow.EmployeeStatusesRepository.GetAll().ToList();

            var employementStatuses = new List<SelectListItem>();

            if (includeEmpty)
                employementStatuses.Add(new SelectListItem { Value = 0.ToString(), Text = _localizer.SelectEmployementStatus });

            employementStatuses.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return employementStatuses;
        }

        #endregion

        #region Institutions
        public List<SelectListItem> Institutions(bool includeEmpty = true)
        {
            List<Institution> list = _dataUnitOfWork.BaseUow.InstitutionsRepository.GetAll().ToList();
            var institution = new List<SelectListItem>();
            if (includeEmpty)
                institution.Add(new SelectListItem { Value = string.Empty, Text = _localizer.ChooseInstitution });
            institution.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            return institution;
        }
        public List<SelectListItem> InstitutionTypes(bool includeEmpty = true)
        {
            List<InstitutionType> list = _dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetAll().ToList();
            var institutionTypes = new List<SelectListItem>();
            if (includeEmpty)
                institutionTypes.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectInstitutionType });
            institutionTypes.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            return institutionTypes;
        }
        #endregion

    }
}