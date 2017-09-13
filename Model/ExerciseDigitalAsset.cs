using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class ExerciseDigitalAsset: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("Exercise")]
        public int? ExerciseId { get; set; }

        [ForeignKey("DigitalAsset")]
        public int? DigitalAssetId { get; set; }

        [Index("ExerciseDigitalAssetNameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]     
        [StringLength(MaxStringLength)]		   
		public string Name { get; set; }
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Exercise Exercise { get; set; }

        public virtual DigitalAsset DigitalAsset { get; set; }
    }
}
