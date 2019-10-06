using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace nevladinaOrg.Web.Helpers.SelectListHelper
{
    public interface ISelectListHelper
    {
        List<SelectListItem> Countries(bool includeEmpty = false);
        List<SelectListItem> LicenseTypes(bool includeEmpty = true);
        List<SelectListItem> RegionsByCountryID(int? countryId, bool includeEmpty = false);
        List<SelectListItem> CitiesByCountryID(int? countryId, bool includeEmpty = false);
        List<SelectListItem> CitiesByRegionID(int regionId, bool includeEmpty = false);
        List<SelectListItem> Regions(bool includeEmpty = false);
        List<SelectListItem> Cities(bool includeEmpty = false);
        List<SelectListItem> Sponsors(bool includeEmpty = true);
        List<SelectListItem> SponsorTypes(bool includeEmpty = true);
        List<SelectListItem> EventItemTypes(bool includeEmpty = true);
        List<SelectListItem> Lecturers(bool includeEmpty = true);
        List<SelectListItem> Persons(bool includeEmpty = true);
        List<SelectListItem> Citizenships(bool includeEmpty = true);
        List<SelectListItem> MartialStatuses(bool includeEmpty = false);
        List<SelectListItem> AcademicDegrees(bool includeEmpty = false);
        List<SelectListItem> AcademicTitles(bool includeEmpty = false);
        List<SelectListItem> Roles(bool includeEmpty = false);
        List<SelectListItem> CountriesBase(bool includeEmpty = false);
        List<SelectListItem> CitiesBase(bool includeEmpty = false);
        List<SelectListItem> CitiesBaseByCountryID(int? countryId, bool includeEmpty = false);
        List<SelectListItem> EmployementStatuses(bool includeEmpty = false);
        List<SelectListItem> AnnouncementTypes(bool includeEmpty = true);
        List<SelectListItem> Organizations(bool includeEmpty = true);
    }
}