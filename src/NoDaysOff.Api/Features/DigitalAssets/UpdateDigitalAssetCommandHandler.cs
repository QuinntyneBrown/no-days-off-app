using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateDigitalAssetCommandHandler : IRequestHandler<UpdateDigitalAssetCommand, DigitalAssetDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateDigitalAssetCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DigitalAssetDto> Handle(UpdateDigitalAssetCommand request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets
            .FirstOrDefaultAsync(x => x.Id == request.DigitalAssetId, cancellationToken)
            ?? throw new InvalidOperationException($"DigitalAsset with id {request.DigitalAssetId} not found.");

        digitalAsset.UpdateName(request.Name, request.ModifiedBy);

        if (request.Description is not null)
        {
            digitalAsset.UpdateDescription(request.Description, request.ModifiedBy);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return digitalAsset.ToDto();
    }
}
