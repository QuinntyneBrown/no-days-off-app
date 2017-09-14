using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class Athlete: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
		[Index("AthleteNameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]     
        [StringLength(MaxStringLength)]		   
		public string Name { get; set; }

        public string Username { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<CompletedScheduledExercise> CompletedScheduledExercises { get; set; } = new HashSet<CompletedScheduledExercise>();

        public ICollection<AthleteWeight> AthleteWeights { get; set; } = new HashSet<AthleteWeight>();

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
