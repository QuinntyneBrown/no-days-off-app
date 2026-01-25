using MediatR;

namespace Athletes.Core.Features.Athletes.DeleteAthlete;

public sealed record DeleteAthleteCommand(int AthleteId, string DeletedBy) : IRequest<bool>;
