using MediatR;

namespace Api;

public sealed record UpdateProfileCommand(
    int ProfileId,
    string Name,
    string Username,
    string ModifiedBy) : IRequest<ProfileDto>;
