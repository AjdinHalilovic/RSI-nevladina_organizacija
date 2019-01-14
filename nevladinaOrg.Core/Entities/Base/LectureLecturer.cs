using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.LectureLecturers)]
    public class LectureLecturer : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,
         ForeignKey(nameof(Lecture))]
        public int LectureId { get; set; }
        [Required,
         ForeignKey(nameof(Lecturer))]
        public int LecturerId { get; set; }
        public bool IsDeleted { get; set; }
        public Lecture Lecture { get; set; }
        public Lecturer Lecturer { get; set; }
    }
}
