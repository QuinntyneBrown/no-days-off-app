using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.BodyPartAggregate;

namespace NoDaysOff.Api;

public sealed class CreateBodyPartCommandHandler : IRequestHandler<CreateBodyPartCommand, BodyPartDto>
{
    private readonly INoDaysOffContext _context;

    public CreateBodyPartCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<BodyPartDto> Handle(CreateBodyPartCommand request, CancellationToken cancellationToken)
    {
        var bodyPart = BodyPart.Create(request.TenantId, request.Name, request.CreatedBy);

        _context.BodyParts.Add(bodyPart);

        await _context.SaveChangesAsync(cancellationToken);

        return bodyPart.ToDto();
    }
}
