using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteBodyPartCommand(int BodyPartId, string DeletedBy) : IRequest;
