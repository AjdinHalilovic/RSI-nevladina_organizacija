using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NevladinaOrganizacija.Areas.Administration
{
    [Table("Citizenships")]
    public class Citizenship
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Country field is required!"),StringLength(30,MinimumLength = 2,ErrorMessage="Country name range is from 2 to 30 characters!")]
        public string Country { get; set; }
        [Required(ErrorMessage="Name field is required!"), StringLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage="Nationality field is required!"), StringLength(40)]
        public string Nationality { get; set; }
    }
}