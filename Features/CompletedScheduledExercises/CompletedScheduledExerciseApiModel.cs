using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{
    public class CompletedScheduledExerciseApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromCompletedScheduledExercise<TModel>(CompletedScheduledExercise completedScheduledExercise) where
            TModel : CompletedScheduledExerciseApiModel, new()
        {
            var model = new TModel();
            model.Id = completedScheduledExercise.Id;
            model.TenantId = completedScheduledExercise.TenantId;            
            return model;
        }

        public static CompletedScheduledExerciseApiModel FromCompletedScheduledExercise(CompletedScheduledExercise completedScheduledExercise)
            => FromCompletedScheduledExercise<CompletedScheduledExerciseApiModel>(completedScheduledExercise);

    }
}
