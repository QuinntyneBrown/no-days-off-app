using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteProfileCommand(int ProfileId, string DeletedBy) : IRequest;
