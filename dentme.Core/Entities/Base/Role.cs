using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Roles)]
    public class Role : IEntity
    {
        [Key, 
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? InstitutionId { get; set; }

        public int? OrganizationId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool Active { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<RoleFunctionality> RoleFunctionalities { get; set; }
    }
}