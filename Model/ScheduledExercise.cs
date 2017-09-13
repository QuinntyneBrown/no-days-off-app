using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class ScheduledExercise: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("Day")]
        public int? DayId { get; set; }

        [ForeignKey("Exercise")]
        public int? ExerciseId { get; set; }

        public int Sort { get; set; } = 0;

        public int Reps { get; set; }

        public int WeightInKgs { get; set; }

        public int Sets { get; set; }

        public int Distance { get; set; }

        public int TimeInSeconds { get; set; }

        [Index("ScheduledExerciseNameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]     
        [StringLength(MaxStringLength)]		   
		public string Name { get; set; }
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Day Day { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}
