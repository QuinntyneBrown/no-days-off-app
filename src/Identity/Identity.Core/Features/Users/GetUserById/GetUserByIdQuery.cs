using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(int UserId) : IRequest<UserDto?>;
