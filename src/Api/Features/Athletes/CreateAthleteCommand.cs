using MediatR;

namespace Api;

public sealed record CreateAthleteCommand(
    int TenantId,
    string Name,
    string Username,
    string CreatedBy) : IRequest<AthleteDto>;
