using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Users)]
    public class User : IEntity
    {
        [Key,
         ForeignKey(nameof(Person)),
         DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,
         StringLength(100)]
        public string PasswordHash { get; set; }

        [Required,
         StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required,
         StringLength(20)]
        public string CultureName { get; set; }

        [Required,
         DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public bool ChangedPassword { get; set; }


        public bool IsDeleted { get; set; }

        
        public Person Person { get; set; }
    }
}
