using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateBodyPartCommandHandler : IRequestHandler<UpdateBodyPartCommand, BodyPartDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateBodyPartCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<BodyPartDto> Handle(UpdateBodyPartCommand request, CancellationToken cancellationToken)
    {
        var bodyPart = await _context.BodyParts
            .FirstOrDefaultAsync(x => x.Id == request.BodyPartId, cancellationToken)
            ?? throw new InvalidOperationException($"BodyPart with id {request.BodyPartId} not found.");

        bodyPart.UpdateName(request.Name, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return bodyPart.ToDto();
    }
}
