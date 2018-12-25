using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.InstitutionTypes)]
    public class InstitutionType : IEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
