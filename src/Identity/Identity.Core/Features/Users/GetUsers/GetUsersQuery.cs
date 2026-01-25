using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Users.GetUsers;

public sealed record GetUsersQuery(int? TenantId = null) : IRequest<IEnumerable<UserDto>>;
