namespace Api;

public sealed record DayDto(
    int DayId,
    string Name,
    IEnumerable<int> BodyPartIds,
    DateTime CreatedOn,
    string CreatedBy);
