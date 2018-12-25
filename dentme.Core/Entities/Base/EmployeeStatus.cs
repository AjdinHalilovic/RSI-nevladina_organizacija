using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.EmployeeStatuses)]
    public class EmployeeStatus:IEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,StringLength(100)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
