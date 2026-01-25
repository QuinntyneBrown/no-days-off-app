using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<AuthResponseDto>;
