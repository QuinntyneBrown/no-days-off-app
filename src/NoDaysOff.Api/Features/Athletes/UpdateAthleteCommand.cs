using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateAthleteCommand(
    int AthleteId,
    string Name,
    string Username,
    string ModifiedBy) : IRequest<AthleteDto>;
