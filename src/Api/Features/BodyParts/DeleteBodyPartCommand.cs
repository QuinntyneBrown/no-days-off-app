using MediatR;

namespace Api;

public sealed record DeleteBodyPartCommand(int BodyPartId, string DeletedBy) : IRequest;
