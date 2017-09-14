using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Athletes
{
    public class AthleteApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }

        public static TModel FromAthlete<TModel>(Athlete athlete) where
            TModel : AthleteApiModel, new()
        {
            var model = new TModel();
            model.Id = athlete.Id;
            model.TenantId = athlete.TenantId;
            model.Name = athlete.Name;
            model.Username = athlete.Username;
            model.ImageUrl = athlete.ImageUrl;
            return model;
        }

        public static AthleteApiModel FromAthlete(Athlete athlete)
            => FromAthlete<AthleteApiModel>(athlete);

    }
}
