using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.ScheduledExercises
{
    public class ScheduledExerciseApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromScheduledExercise<TModel>(ScheduledExercise scheduledExercise) where
            TModel : ScheduledExerciseApiModel, new()
        {
            var model = new TModel();
            model.Id = scheduledExercise.Id;
            model.TenantId = scheduledExercise.TenantId;
            model.Name = scheduledExercise.Name;
            return model;
        }

        public static ScheduledExerciseApiModel FromScheduledExercise(ScheduledExercise scheduledExercise)
            => FromScheduledExercise<ScheduledExerciseApiModel>(scheduledExercise);

    }
}
