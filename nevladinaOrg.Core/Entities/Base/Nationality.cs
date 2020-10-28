using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Nationalities)]
    public class Nationality : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required,
         StringLength(50)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
