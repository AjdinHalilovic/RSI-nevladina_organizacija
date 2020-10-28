using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class ContactTypeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator ContactType(ContactTypeViewModel model)
        {
            ContactType contactType = new ContactType()
            {
                Id = model.Id,
                Name = model.Name
            };

            return contactType;
        }

        public static implicit operator ContactTypeViewModel(ContactType model)
        {
            ContactTypeViewModel contactTypeViewModel = new ContactTypeViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return contactTypeViewModel;
        }
    }
}
