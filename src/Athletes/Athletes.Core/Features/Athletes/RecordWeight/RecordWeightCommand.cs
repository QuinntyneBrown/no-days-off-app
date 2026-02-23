using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.RecordWeight;

public sealed record RecordWeightCommand(
    int AthleteId,
    int WeightInKgs,
    DateTime WeighedOn,
    string RecordedBy) : IRequest<AthleteDto>;
