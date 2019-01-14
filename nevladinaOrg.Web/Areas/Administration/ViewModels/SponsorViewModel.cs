using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class SponsorViewModel
    {
        public int Id { get; set; }
        public int SponsorId { get; set; }
        public int EventId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageSponsorTypeReq))]
        public int SponsorTypeId { get; set; }
        public string EventName { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageNewSponsorReq))]
        public string NewSponsor { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string WebUrl { get; set; }
        public string Description { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

        public static implicit operator Sponsor(SponsorViewModel model)
        {
            Sponsor bloodType = new Sponsor()
            {
                Id = model.SponsorId,
                Name = model.Name,
                Logo=model.Logo,
                WebUrl=model.WebUrl,
                Description=model.Description
            };

            return bloodType;
        }
        public static implicit operator SponsorViewModel(Sponsor model)
        {
            SponsorViewModel bloodType = new SponsorViewModel()
            {
                SponsorId = model.Id,
                Name = model.Name,
                Logo = model.Logo,
                WebUrl = model.WebUrl,
                Description = model.Description
            };

            return bloodType;
        }
    }
}
