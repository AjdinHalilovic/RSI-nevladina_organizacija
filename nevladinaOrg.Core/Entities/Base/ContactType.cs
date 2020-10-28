using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.ContactTypes)]
    public class ContactType
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,
         StringLength(50)]
        public string Name { get; set; }

        //[Required]
        public string Icon { get; set; }

        public bool Active { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
