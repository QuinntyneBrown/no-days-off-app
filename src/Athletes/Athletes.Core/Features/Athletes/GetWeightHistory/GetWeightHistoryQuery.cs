using MediatR;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetWeightHistory;

public sealed record GetWeightHistoryQuery(
    int AthleteId,
    int Count = 10) : IRequest<IEnumerable<AthleteWeightDto>>;
