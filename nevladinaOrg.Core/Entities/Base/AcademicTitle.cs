﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.AcademicTitles)]
    public class AcademicTitle : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
