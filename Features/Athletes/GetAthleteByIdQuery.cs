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
    public class GetAthleteByIdQuery
    {
        public class Request : BaseRequest, IRequest<Response> { 
            public int Id { get; set; }            
        }

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
                return new Response()
                {
                    Athlete = AthleteApiModel.FromAthlete(await _context.Athletes
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }

    }

}
