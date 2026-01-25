using MediatR;
using Core;
using Core.Model.DayAggregate;

namespace Api;

public sealed class CreateDayCommandHandler : IRequestHandler<CreateDayCommand, DayDto>
{
    private readonly INoDaysOffContext _context;

    public CreateDayCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DayDto> Handle(CreateDayCommand request, CancellationToken cancellationToken)
    {
        var day = Day.Create(request.TenantId, request.Name, request.CreatedBy);

        _context.Days.Add(day);

        await _context.SaveChangesAsync(cancellationToken);

        return day.ToDto();
    }
}
