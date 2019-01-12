using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class PersonDetailsViewModel
    {
        public int Id { get; set; }
        public int? MartialStatusId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =nameof(Localizer.ErrorMessageAcademicDegreeReq))]
        public int AcademicDegreeId { get; set; }
        public int? AcademicTitleId { get; set; }
        public int? EmploymentStatusId { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator PersonDetail(PersonDetailsViewModel model)
        {
            PersonDetail personDetail = new PersonDetail()
            {
                Id = model.Id,
                AcademicDegreeId=model.AcademicDegreeId,
                AcademicTitleId=model.AcademicTitleId,
                EmploymentStatusId=model.EmploymentStatusId,
                MartialStatusId=model.MartialStatusId
            };

            return personDetail;
        }

        public static implicit operator PersonDetailsViewModel(PersonDetail model)
        {
            PersonDetailsViewModel personDetail = new PersonDetailsViewModel()
            {
                Id = model.Id,
                AcademicDegreeId = model.AcademicDegreeId,
                AcademicTitleId = model.AcademicTitleId,
                EmploymentStatusId = model.EmploymentStatusId,
                MartialStatusId = model.MartialStatusId
            };

            return personDetail;
        }
    }
}
