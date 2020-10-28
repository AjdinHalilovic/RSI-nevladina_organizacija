using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.OrganizationInstitutionUsers)]
    public class OrganizationInstitutionUser : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Token { get; set; }

        [ForeignKey(nameof(InstitutionUser))]
        public int InstitutionUserId { get; set; }

        [ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }
        public bool Employed { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }

        public Organization Organization { get; set; }
        public InstitutionUser InstitutionUser { get; set; }
    }
}
