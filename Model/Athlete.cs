using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class Athlete: Profile
    {

        public ICollection<CompletedScheduledExercise> CompletedScheduledExercises { get; set; } = new HashSet<CompletedScheduledExercise>();

        public ICollection<AthleteWeight> AthleteWeights { get; set; } = new HashSet<AthleteWeight>();

        public int? CurrentWeight { get; set; }

        public DateTime? LastWeighedOn { get; set; }
        
    }
}
