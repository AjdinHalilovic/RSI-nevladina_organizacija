using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Functionalities)]
    public class Functionality : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, 
         StringLength(100)]
        public string Name { get; set; }

        [Required, 
         StringLength(100)]
        public string ControllerName { get; set; }

        [Required, 
         Range(1, int.MaxValue)]
        public int FunctionNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}