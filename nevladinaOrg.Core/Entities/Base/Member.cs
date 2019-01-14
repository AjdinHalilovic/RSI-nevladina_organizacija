using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Members)]
    public class Member : IEntity
    {
        [Key,
         ForeignKey(nameof(OrganizationInstitutionUser)),
         DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Note { get; set; }
        public bool IsDeleted { get; set; }


        public OrganizationInstitutionUser OrganizationInstitutionUser { get; set; }
    }
}
