using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.PersonDetails)]
    public class PersonDetail : IEntity
    {
        [Key, ForeignKey(nameof(Person)), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ForeignKey(nameof(MaritalStatus))]
        public int? MartialStatusId { get; set; }
        [ForeignKey(nameof(AcademicDegree))]
        public int AcademicDegreeId { get; set; }
        [ForeignKey(nameof(AcademicTitle))]
        public int? AcademicTitleId { get; set; }
        [ForeignKey(nameof(EmployeeStatus))]
        public int? EmploymentStatusId { get; set; }

        public bool IsDeleted { get; set; }


        public Person Person { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public AcademicDegree AcademicDegree { get; set; }
        public AcademicTitle AcademicTitle { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
    }
}
