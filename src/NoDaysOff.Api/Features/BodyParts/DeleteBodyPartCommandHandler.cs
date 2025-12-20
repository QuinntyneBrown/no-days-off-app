using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteBodyPartCommandHandler : IRequestHandler<DeleteBodyPartCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteBodyPartCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteBodyPartCommand request, CancellationToken cancellationToken)
    {
        var bodyPart = await _context.BodyParts
            .FirstOrDefaultAsync(x => x.Id == request.BodyPartId, cancellationToken)
            ?? throw new InvalidOperationException($"BodyPart with id {request.BodyPartId} not found.");

        bodyPart.Delete(request.DeletedBy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
