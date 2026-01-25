using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetAthleteById;

public sealed record GetAthleteByIdQuery(int AthleteId) : IRequest<AthleteDto?>;
