using NoDaysOffApp.Model;
using System;

namespace NoDaysOffApp.Features.AthleteWeights
{
    public class AthleteWeightApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public int WeightInKgs { get; set; }
        public DateTime WeighedOn { get; set; }

        public static TModel FromAthleteWeight<TModel>(AthleteWeight athleteWeight) where
            TModel : AthleteWeightApiModel, new()
        {
            var model = new TModel();
            model.Id = athleteWeight.Id;
            model.TenantId = athleteWeight.TenantId;
            model.WeightInKgs = athleteWeight.WeightInKgs;
            model.WeighedOn = athleteWeight.WeighedOn;
            
            return model;
        }

        public static AthleteWeightApiModel FromAthleteWeight(AthleteWeight athleteWeight)
            => FromAthleteWeight<AthleteWeightApiModel>(athleteWeight);

    }
}
