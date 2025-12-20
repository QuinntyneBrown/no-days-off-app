using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetProfilesQueryHandler : IRequestHandler<GetProfilesQuery, IEnumerable<ProfileDto>>
{
    private readonly INoDaysOffContext _context;

    public GetProfilesQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProfileDto>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Profiles
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
