using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.ProfileAggregate;

namespace NoDaysOff.Api;

public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ProfileDto>
{
    private readonly INoDaysOffContext _context;

    public CreateProfileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = Profile.Create(request.TenantId, request.Name, request.Username, request.CreatedBy);

        _context.Profiles.Add(profile);

        await _context.SaveChangesAsync(cancellationToken);

        return profile.ToDto();
    }
}
