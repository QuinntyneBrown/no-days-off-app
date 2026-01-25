using MediatR;
using Shared.Contracts.Workouts;

namespace Workouts.Core.Features.Workouts.GetWorkouts;

public sealed record GetWorkoutsQuery(int? TenantId = null) : IRequest<IEnumerable<WorkoutDto>>;
