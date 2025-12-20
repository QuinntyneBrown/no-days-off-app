using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteDigitalAssetCommandHandler : IRequestHandler<DeleteDigitalAssetCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteDigitalAssetCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDigitalAssetCommand request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets
            .FirstOrDefaultAsync(x => x.Id == request.DigitalAssetId, cancellationToken)
            ?? throw new InvalidOperationException($"DigitalAsset with id {request.DigitalAssetId} not found.");

        digitalAsset.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
