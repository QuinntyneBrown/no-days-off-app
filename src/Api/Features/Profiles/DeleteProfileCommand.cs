using MediatR;

namespace Api;

public sealed record DeleteProfileCommand(int ProfileId, string DeletedBy) : IRequest;
