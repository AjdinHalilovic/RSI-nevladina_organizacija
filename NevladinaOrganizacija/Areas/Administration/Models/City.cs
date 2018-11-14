using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NevladinaOrganizacija.Areas.Administration
{
    [Table("Cities")]
    public class City
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,
         StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string PostalCode { get; set; }

        public bool IsDeleted { get; set; }
    }
}
