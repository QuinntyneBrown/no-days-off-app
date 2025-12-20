using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteVideoCommand(int VideoId, string DeletedBy) : IRequest;
