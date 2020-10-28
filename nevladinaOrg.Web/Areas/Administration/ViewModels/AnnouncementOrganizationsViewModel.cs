using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class AnnouncementOrganizationsViewModel
    {
        public AnnouncementViewModel Announcement { get; set; }
        public Dictionary<string,string> SrcFiles{ get; set; }
        public IEnumerable<AnnouncementDocument> Documents { get; set; }
        public IEnumerable<AnnouncementPhoto> Photos { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageCheckSendToReq))]
        public string SendArea { get; set; }
        public string SendToOrganizations { get; set; }
        public string SendToUsers { get; set; }
        public IEnumerable<string> OrganizationIds { get; set; }
        public IEnumerable<string> UserIds { get; set; }
        public List<AnnouncementPhoto> AnnouncementPhotos { get; set; }
        public List<AnnouncementDocument> AnnouncementDocuments { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }
    }
}
