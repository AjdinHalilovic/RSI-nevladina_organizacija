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
    public class AnnouncementPhotosViewModel
    {
        [Key]
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Thumbnail { get; set; }
        public static implicit operator AnnouncementPhoto(AnnouncementPhotosViewModel model)
        {
            AnnouncementPhoto announcementPhoto = new AnnouncementPhoto()
            {
                Id = model.Id,
                AnnouncementId=model.AnnouncementId,
                Name = model.Name,
                Type=model.Type,
                Size=model.Size,
                Photo=model.Photo,
                Thumbnail=model.Thumbnail
            };

            return announcementPhoto;
        }

        public static implicit operator AnnouncementPhotosViewModel(AnnouncementPhoto model)
        {
            AnnouncementPhotosViewModel announcementPhoto = new AnnouncementPhotosViewModel()
            {
                Id = model.Id,
                AnnouncementId = model.AnnouncementId,
                Name = model.Name,
                Type = model.Type,
                Size = model.Size,
                Photo = model.Photo,
                Thumbnail = model.Thumbnail
            };

            return announcementPhoto;
        }
    }
}
