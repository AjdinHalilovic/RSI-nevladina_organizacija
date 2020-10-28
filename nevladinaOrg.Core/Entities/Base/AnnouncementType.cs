using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AnnouncementType)]
    public class AnnouncementType : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50), Required]
        public string Name { get; set; }
        public string Icon { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
