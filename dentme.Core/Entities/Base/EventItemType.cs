using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EventItemTypes)]
    public class EventItemType:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,
         ForeignKey(nameof(Institution))]
        public int InstitutionId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public Institution Institution { get; set; }
    }
}
