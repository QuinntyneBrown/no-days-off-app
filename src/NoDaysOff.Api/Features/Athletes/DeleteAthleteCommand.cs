using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteAthleteCommand(int AthleteId, string DeletedBy) : IRequest;
