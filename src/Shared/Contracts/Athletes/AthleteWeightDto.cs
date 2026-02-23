namespace Shared.Contracts.Athletes;

public sealed record AthleteWeightDto(
    int Id,
    int WeightInKgs,
    DateTime WeighedOn,
    string RecordedBy);

public sealed record RecordWeightDto(
    int WeightInKgs,
    DateTime WeighedOn);
