using MediatR;

namespace Api;

public sealed record DeleteAthleteCommand(int AthleteId, string DeletedBy) : IRequest;
