using MediatR;

namespace NoDaysOff.Api;

public sealed record GetAthleteByIdQuery(int AthleteId) : IRequest<AthleteDto?>;
