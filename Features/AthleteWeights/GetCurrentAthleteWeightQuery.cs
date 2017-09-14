using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.AthleteWeights
{
    public class GetCurrentAthleteWeightQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public AthleteWeightApiModel AthleteWeight { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(NoDaysOffAppContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request)
            {
                var athleteWeight = await _context.AthleteWeights
                    .OrderByDescending(x => x.WeighedOn)
                    .FirstOrDefaultAsync();

                return new Response()
                {
                    AthleteWeight = athleteWeight != null ? AthleteWeightApiModel.FromAthleteWeight(athleteWeight) : null
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
