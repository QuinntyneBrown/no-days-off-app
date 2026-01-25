using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.UpdateAthlete;

public sealed record UpdateAthleteCommand(
    int AthleteId,
    string Name,
    string Username,
    string ModifiedBy) : IRequest<AthleteDto>;
