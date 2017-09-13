using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Days
{
    public class DayApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromDay<TModel>(Day day) where
            TModel : DayApiModel, new()
        {
            var model = new TModel();
            model.Id = day.Id;
            model.TenantId = day.TenantId;
            model.Name = day.Name;
            return model;
        }

        public static DayApiModel FromDay(Day day)
            => FromDay<DayApiModel>(day);

    }
}
