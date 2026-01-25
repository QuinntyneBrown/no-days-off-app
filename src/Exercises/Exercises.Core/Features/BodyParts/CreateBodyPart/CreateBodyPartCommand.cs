using MediatR;
using Shared.Contracts.Exercises;
using Exercises.Core.Aggregates.BodyPart;

namespace Exercises.Core.Features.BodyParts.CreateBodyPart;

public record CreateBodyPartCommand(string Name, int TenantId) : IRequest<BodyPartDto>;

public class CreateBodyPartHandler : IRequestHandler<CreateBodyPartCommand, BodyPartDto>
{
    private readonly IExercisesDbContext _context;

    public CreateBodyPartHandler(IExercisesDbContext context)
    {
        _context = context;
    }

    public async Task<BodyPartDto> Handle(CreateBodyPartCommand request, CancellationToken cancellationToken)
    {
        var bodyPart = new BodyPart(request.Name, request.TenantId);

        _context.BodyParts.Add(bodyPart);
        await _context.SaveChangesAsync(cancellationToken);

        return new BodyPartDto(
            bodyPart.Id,
            bodyPart.Name,
            bodyPart.TenantId,
            bodyPart.CreatedAt
        );
    }
}
