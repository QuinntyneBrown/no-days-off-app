using MediatR;
using Core;
using Core.Model.DigitalAssetAggregate;

namespace Api;

public sealed class CreateDigitalAssetCommandHandler : IRequestHandler<CreateDigitalAssetCommand, DigitalAssetDto>
{
    private readonly INoDaysOffContext _context;

    public CreateDigitalAssetCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DigitalAssetDto> Handle(CreateDigitalAssetCommand request, CancellationToken cancellationToken)
    {
        var digitalAsset = DigitalAsset.Create(
            request.Name,
            request.FileName,
            request.ContentType,
            request.Size,
            request.CreatedBy);

        _context.DigitalAssets.Add(digitalAsset);

        await _context.SaveChangesAsync(cancellationToken);

        return digitalAsset.ToDto();
    }
}
