using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.InstitutionContacts)]
    public class InstitutionContact : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Institution))]
        public int InstitutionId { get; set; }

        [Required]
        public int ContactTypeId { get; set; }

        [Required]
        public string Contact { get; set; }

        public bool IsDeleted { get; set; }

        
        public Institution Institution { get; set; }
    }
}
