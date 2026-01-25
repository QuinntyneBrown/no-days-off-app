namespace Api;

public sealed record ExerciseDto(
    int ExerciseId,
    string Name,
    int? BodyPartId,
    int DefaultSets,
    int DefaultRepetitions,
    IEnumerable<int> DigitalAssetIds,
    DateTime CreatedOn,
    string CreatedBy);
