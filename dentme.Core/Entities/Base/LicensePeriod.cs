using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.LicensePeriods)]
    public class LicensePeriod : IEntity
    {
        [Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(MemberLicense))]
        public int MemberLicenseId { get; set; }
        [ForeignKey(nameof(LicenseType))]
        public int LicenseTypeId { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

        public MemberLicense MemberLicense { get; set; }
        public LicenseType LicenseType { get; set; }
    }
}
