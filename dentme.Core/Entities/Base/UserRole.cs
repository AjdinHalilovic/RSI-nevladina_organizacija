using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.UserRoles)]
    public class UserRole : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(OrganizationInstitutionUser))]
        public int? OrganizationInstitutionUserId { get; set; }
        [ForeignKey(nameof(InstitutionUser))]
        public int InstitutionUserId { get; set; }
        [Required, ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public bool IsDeleted { get; set; }


        public OrganizationInstitutionUser OrganizationInstitutionUser { get; set; }
        public InstitutionUser InstitutionUser { get; set; }
        public Role Role { get; set; }
    }
}