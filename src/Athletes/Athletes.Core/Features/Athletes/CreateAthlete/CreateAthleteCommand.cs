using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.CreateAthlete;

public sealed record CreateAthleteCommand(
    string Name,
    string Username,
    int? TenantId,
    string CreatedBy) : IRequest<AthleteDto>;
