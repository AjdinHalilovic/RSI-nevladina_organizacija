using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.PersonContacts)]
    public class PersonContact : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Person))]
        public int PersonId { get; set; }

        [Required,ForeignKey(nameof(ContactType))]
        public int ContactTypeId { get; set; }

        [Required]
        public string Contact { get; set; }

        public bool Primary { get; set; }

        public bool IsDeleted { get; set; }

        public Person Person { get; set; }
        public ContactType ContactType { get; set; }
    }
}
