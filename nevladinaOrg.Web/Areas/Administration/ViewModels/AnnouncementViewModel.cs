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
    public class AnnouncementViewModel
    {
        [Key]
        public int Id { get; set; }
        [Range(1, sizeof(int), ErrorMessage = nameof(Localizer.ErrorMessageAnnouncementTypeReq))]
        public int? AnnouncementTypeId { get; set; }
        public int? UserId { get; set; }
        public int? InstitutionId { get; set; }
        public int? OrganizationId { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageTitleReq)), StringLength(80, ErrorMessage = nameof(Localizer.ErrorMessageTitleLen80))]
        public string Title { get; set; }
        [StringLength(80, ErrorMessage = nameof(Localizer.ErrorMessageSubtitleLen80))]
        public string Subtitle { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageContentReq))]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateString { get; set; }
        public bool Active { get; set; }

        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }
        public static implicit operator Announcement(AnnouncementViewModel model)
        {
            Announcement announcement = new Announcement()
            {
                Id = model.Id,
                UserId = model.UserId,
                InstitutionId = model.InstitutionId,
                OrganizationId = model.OrganizationId,
                AnnouncementTypeId = model.AnnouncementTypeId,
                Title = model.Title,
                Subtitle = model.Subtitle,
                Content = model.Content,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                Active = model.Active
            };

            return announcement;
        }
        public static implicit operator AnnouncementViewModel(Announcement model)
        {
            AnnouncementViewModel announcement = new AnnouncementViewModel()
            {
                Id = model.Id,
                UserId = model.UserId,
                InstitutionId = model.InstitutionId,
                OrganizationId = model.OrganizationId,
                AnnouncementTypeId = model.AnnouncementTypeId,
                Title = model.Title,
                Subtitle = model.Subtitle,
                Content = model.Content,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                Active = model.Active
            };

            return announcement;
        }
    }
}
