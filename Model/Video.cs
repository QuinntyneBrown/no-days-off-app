using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class Video: ILoggable
    {        
		[Index("VideoTitleIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]     
        [StringLength(MaxStringLength)]		   
		public string Title { get; set; }
        public int Id { get; set; }
        [ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        public string Category { get; set; }
        public string SubTitle { get; set; }
        public string Slug { get; set; }
        public string YouTubeVideoId { get; set; }
        public string Abstract { get; set; }
        public int DurationInSeconds { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Tenant Tenant { get; set; }
        public bool IsDeleted { get; set; }
    }
}
