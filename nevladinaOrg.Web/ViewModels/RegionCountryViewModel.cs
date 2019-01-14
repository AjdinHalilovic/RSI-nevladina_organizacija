using Core.Entities.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace nevladinaOrg.Web.ViewModels
{
    public class RegionCountryViewModel
    {
        public RegionCountryViewModel(IEnumerable<Region> regions, IEnumerable<SelectListItem> selectListCountries)
        {
            this.regions = regions;
            this.selectListCountries = selectListCountries;
        }
        public IEnumerable<Region> regions{ get; set; }
        public IEnumerable<SelectListItem> selectListCountries { get; set; }
    }
}
