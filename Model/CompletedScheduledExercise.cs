using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class CompletedScheduledExercise: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("ScheduledExercise")]
        public int? ScheduledExerciseId { get; set; }

        [ForeignKey("Athlete")]
        public int? AthleteId { get; set; }

        public int WeightInKgs { get; set; }

        public int Reps { get; set; }

        public int Sets { get; set; }

        public int Distance { get; set; }

        public int TimeInSeconds { get; set; }

        public DateTime CompletitonDateTime { get; set; }
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ScheduledExercise ScheduledExercise { get; set; }

        public virtual Athlete Athlete { get; set; }
    }
}
