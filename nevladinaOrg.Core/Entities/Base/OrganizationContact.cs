using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.OrganizationContacts)]
    public class OrganizationContact : IEntity
    {
        [Key, 
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }

        [Required]
        public int ContactTypeId { get; set; }

        [Required]
        public string Contact { get; set; }

        public bool IsDeleted { get; set; }

        public Organization Organization { get; set; }
    }
}
