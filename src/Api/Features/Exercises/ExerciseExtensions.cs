using Core.Model.ExerciseAggregate;

namespace Api;

public static class ExerciseExtensions
{
    public static ExerciseDto ToDto(this Exercise exercise)
    {
        return new ExerciseDto(
            exercise.Id,
            exercise.Name,
            exercise.BodyPartId,
            exercise.DefaultSets,
            exercise.DefaultRepetitions,
            exercise.DigitalAssetIds,
            exercise.CreatedOn,
            exercise.CreatedBy);
    }
}
