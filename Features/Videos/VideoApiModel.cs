using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Videos
{
    public class VideoApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromVideo<TModel>(Video video) where
            TModel : VideoApiModel, new()
        {
            var model = new TModel();
            model.Id = video.Id;
            model.TenantId = video.TenantId;
            model.Name = video.Name;
            return model;
        }

        public static VideoApiModel FromVideo(Video video)
            => FromVideo<VideoApiModel>(video);

    }
}
