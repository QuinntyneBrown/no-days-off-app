using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Users.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IIdentityDbContext _context;

    public GetUserByIdQueryHandler(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

        return user?.ToDto();
    }
}
