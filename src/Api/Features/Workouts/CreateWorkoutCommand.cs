using MediatR;

namespace Api;

public sealed record CreateWorkoutCommand(
    int TenantId,
    string CreatedBy) : IRequest<WorkoutDto>;
