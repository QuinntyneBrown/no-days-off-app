using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Athletes
{
    public class GetCurrentAthleteQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public AthleteApiModel Athlete { get; set; }
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
                var athletes = await _context.Athletes.ToListAsync();

                var athlete = await _context.Athletes.SingleOrDefaultAsync(x => x.Username == request.Username);

                if (athlete == null)
                    return new Response();

                return new Response()
                {
                    Athlete = AthleteApiModel.FromAthlete(athlete)
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
