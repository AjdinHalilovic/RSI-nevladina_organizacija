using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.Lectures)]
      public class Lecture:IEntity
    {
        [Key,
         ForeignKey(nameof(EventItem)),
         DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string AboutLecture { get; set; }
        public bool IsDeleted { get; set; }

        public EventItem EventItem { get; set; }

    }
}
