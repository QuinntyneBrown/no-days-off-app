using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Auth.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    int? TenantId = null) : IRequest<AuthResponseDto>;
