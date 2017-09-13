using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Exercises
{
    public class ExerciseApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromExercise<TModel>(Exercise exercise) where
            TModel : ExerciseApiModel, new()
        {
            var model = new TModel();
            model.Id = exercise.Id;
            model.TenantId = exercise.TenantId;
            model.Name = exercise.Name;
            return model;
        }

        public static ExerciseApiModel FromExercise(Exercise exercise)
            => FromExercise<ExerciseApiModel>(exercise);

    }
}
