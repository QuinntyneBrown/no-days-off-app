using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetAthletes;

public sealed record GetAthletesQuery(int? TenantId = null) : IRequest<IEnumerable<AthleteDto>>;
