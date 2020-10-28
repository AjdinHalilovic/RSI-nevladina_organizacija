using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.MemberEmployements)]
    public class MemberEmployement : IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        [ForeignKey(nameof(Institution))]
        public int? InstitutionId { get; set; }
        [ForeignKey(nameof(Organization))]
        public int? OrganizationId { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
        public string Position { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

        public Member Member { get; set; }
        public Institution Institution { get; set; }
        public Organization Organization { get; set; }
    }
}
