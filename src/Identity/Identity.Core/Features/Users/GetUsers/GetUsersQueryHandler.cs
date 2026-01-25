using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IIdentityDbContext _context;

    public GetUsersQueryHandler(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted);

        if (request.TenantId.HasValue)
        {
            query = query.Where(u => u.TenantId == request.TenantId.Value);
        }

        var users = await query.ToListAsync(cancellationToken);
        return users.Select(u => u.ToDto());
    }
}
