using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.InstitutionUsers)]
    public class InstitutionUser : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Token { get; set; }

        [ForeignKey(nameof(Institution))]
        public int InstitutionId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        

        [DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }


        public bool IsInstitutionUser { get; set; }
        public bool IsOrganizationUser { get; set; }


        public bool Active { get; set; }
        public bool IsDeleted { get; set; }


        public User User { get; set; }
        public Institution Institution { get; set; }
    }
}
