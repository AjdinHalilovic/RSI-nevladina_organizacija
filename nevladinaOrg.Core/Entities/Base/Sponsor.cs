using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Sponsors)]
      public class Sponsor:IEntity
    {
        [Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string WebUrl { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
