using MediatR;

namespace Identity.Core.Features.Auth.RevokeToken;

public sealed record RevokeTokenCommand(string RefreshToken) : IRequest<bool>;
