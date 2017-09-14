using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Profiles
{
    public class ProfileApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromProfile<TModel>(Profile profile) where
            TModel : ProfileApiModel, new()
        {
            var model = new TModel();
            model.Id = profile.Id;
            model.TenantId = profile.TenantId;
            model.Name = profile.Name;
            return model;
        }

        public static ProfileApiModel FromProfile(Profile profile)
            => FromProfile<ProfileApiModel>(profile);

    }
}
