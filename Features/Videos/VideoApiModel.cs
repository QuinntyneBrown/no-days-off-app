using NoDaysOffApp.Model;
using System;

namespace NoDaysOffApp.Features.Videos
{
    public class VideoApiModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Slug { get; set; }
        public string YouTubeVideoId { get; set; }
        public string Abstract { get; set; }
        public int DurationInSeconds { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }

        public static TModel FromVideo<TModel>(Video video) where
            TModel : VideoApiModel, new()
        {
            var model = new TModel();
            model.Id = video.Id;
            model.Title = video.Title;
            model.SubTitle = video.SubTitle;
            model.Slug = video.Slug;
            model.Abstract = video.Abstract;
            model.Description = video.Description;
            model.Category = video.Category;
            model.YouTubeVideoId = video.YouTubeVideoId;
            model.Rating = video.Rating;
            return model;
        }

        public static VideoApiModel FromVideo(Video video)
            => FromVideo<VideoApiModel>(video);

    }
}
