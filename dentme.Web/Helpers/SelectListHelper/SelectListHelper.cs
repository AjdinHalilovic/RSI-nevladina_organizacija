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

        #region Cities
        public List<SelectListItem> Cities(bool includeEmpty = false)
        {
            List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetAll().ToList();

            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCity });

            cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return cities;
        }
        public List<SelectListItem> CitiesBase(bool includeEmpty = false)
        {
            List<City> list = _dataUnitOfWork.BaseUow.CitiesRepository.GetAll().ToList();

            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCity });

            cities.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return cities;
        }

        public List<SelectListItem> CitiesByCountryID(int? countryId, bool includeEmpty = false)
        {
            var cities = new List<SelectListItem>();

            if (includeEmpty)
                cities.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCity });

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
                cities.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCity });

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
                cities.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCity });

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
                countries.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCountry });

            countries.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return countries;
        }
        public List<SelectListItem> CountriesBase(bool includeEmpty = false)
        {
            List<Country> list = _dataUnitOfWork.BaseUow.CountriesRepository.GetAll().ToList();

            var countries = new List<SelectListItem>();

            if (includeEmpty)
                countries.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCountry });

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
                regions.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectRegion });

            regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return regions;
        }

        public List<SelectListItem> RegionsByCountryID(int? countryId, bool includeEmpty = false)
        {
            var regions = new List<SelectListItem>();

            if (includeEmpty)
                regions.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectRegion });

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
                regions.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectCitizenship });

            regions.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return regions;
        }

        #endregion

        #region AcademicDegrees

        public List<SelectListItem> AcademicDegrees(bool includeEmpty = false)
        {
            List<AcademicDegree> list = _dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetAll().ToList();

            var academicDegrees = new List<SelectListItem>();

            if (includeEmpty)
                academicDegrees.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectAcademicDegree });

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
                academicTitles.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectAcademicTitle });

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
                roles.Add(new SelectListItem { Value = string.Empty, Text = _localizer.SelectRoles });

            roles.AddRange(list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return roles;
        }

        #endregion



    }
}