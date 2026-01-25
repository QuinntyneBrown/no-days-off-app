using MediatR;

namespace Api;

public sealed record GetAthleteByIdQuery(int AthleteId) : IRequest<AthleteDto?>;
