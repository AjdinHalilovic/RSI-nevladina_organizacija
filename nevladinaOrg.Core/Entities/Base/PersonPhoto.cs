using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.PersonPhotos)]
    public class PersonPhoto : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,
         ForeignKey(nameof(Person))]
        public int PersonId { get; set; }

        [Required]
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }

        public bool IsDeleted { get; set; }


        public Person Person { get; set; }
    }
}
