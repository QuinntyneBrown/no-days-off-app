using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateProfileCommand(
    int TenantId,
    string Name,
    string Username,
    string CreatedBy) : IRequest<ProfileDto>;
