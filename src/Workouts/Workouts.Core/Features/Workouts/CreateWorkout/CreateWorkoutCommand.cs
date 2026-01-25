using MediatR;
using Shared.Contracts.Workouts;

namespace Workouts.Core.Features.Workouts.CreateWorkout;

public sealed record CreateWorkoutCommand(
    string Name,
    int? TenantId,
    string CreatedBy) : IRequest<WorkoutDto>;
