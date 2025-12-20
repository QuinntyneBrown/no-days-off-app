using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ProfileDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateProfileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(x => x.Id == request.ProfileId, cancellationToken)
            ?? throw new InvalidOperationException($"Profile with id {request.ProfileId} not found.");

        profile.UpdateName(request.Name, request.ModifiedBy);
        profile.UpdateUsername(request.Username, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return profile.ToDto();
    }
}
