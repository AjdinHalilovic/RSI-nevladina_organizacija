using Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.RoleFunctionalities)]
    public class RoleFunctionality : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, 
         ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        [Required, 
         ForeignKey(nameof(Functionality))]
        public int FunctionalityId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? AssignmentDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }


        public Role Role { get; set; }
        public Functionality Functionality { get; set; }
    }
}