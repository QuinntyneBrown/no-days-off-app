using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateWorkoutCommand(
    int TenantId,
    string CreatedBy) : IRequest<WorkoutDto>;
