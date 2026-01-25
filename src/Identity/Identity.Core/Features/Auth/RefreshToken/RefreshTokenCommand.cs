using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;
