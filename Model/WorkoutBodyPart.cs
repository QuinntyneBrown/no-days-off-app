using NoDaysOffApp.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class WorkoutBodyPart: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
		[ForeignKey("Workout")]
        public int? WorkoutId { get; set; }

        [ForeignKey("BodyPart")]
        public int? BodyPartId { get; set; }

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Workout Workout { get; set; }

        public virtual BodyPart BodyPart { get; set; }
    }
}
