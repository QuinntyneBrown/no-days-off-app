using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteProfileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(x => x.Id == request.ProfileId, cancellationToken)
            ?? throw new InvalidOperationException($"Profile with id {request.ProfileId} not found.");

        profile.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
