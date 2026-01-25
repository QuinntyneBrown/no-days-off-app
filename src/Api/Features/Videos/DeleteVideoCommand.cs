using MediatR;

namespace Api;

public sealed record DeleteVideoCommand(int VideoId, string DeletedBy) : IRequest;
