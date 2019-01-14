using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class PersonPhotoViewModel
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public byte[] Photo { get; set; }
        public IFormFile PhotoFile { get; set; }

        public bool IsDeleted { get; set; }
    }
}
