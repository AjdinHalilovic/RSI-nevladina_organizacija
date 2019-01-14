using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.MemberLicenses)]
    public class MemberLicense:IEntity
    {
        [Key,
         DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        [Required]
        public string LicenceNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Permanent { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }

        public Member Member { get; set; }
    }
}
