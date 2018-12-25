using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    [Table(Constants.Tables.Base.ActivityLogs)]
    public class ActivityLog : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? InstitutionId { get; set; }
        
        public int? OrganizationId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        public int? RowId { get; set; }

        public DateTime OccurredAt { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        [StringLength(200)]
        public string WebBrowser { get; set; }

        [StringLength(200)]
        public string ReferrerUrl { get; set; }

        [StringLength(200)]
        public string ActiveUrl { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(50)]
        public string ActionMethod { get; set; }

        [StringLength(100)]
        public string ExceptionType { get; set; }

        [StringLength(500)]
        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public LogActivity LogActivity { get; set; }
    }
}