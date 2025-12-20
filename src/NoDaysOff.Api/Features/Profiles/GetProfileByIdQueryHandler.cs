using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
{
    private readonly INoDaysOffContext _context;

    public GetProfileByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProfileId && !x.IsDeleted, cancellationToken);

        return profile?.ToDto();
    }
}
