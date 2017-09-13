using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.BodyParts
{
    public class GetBodyPartsQuery
    {
        public class Request : BaseRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<BodyPartApiModel> BodyParts { get; set; } = new HashSet<BodyPartApiModel>();
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
                var bodyParts = await _context.BodyParts
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new Response()
                {
                    BodyParts = bodyParts.Select(x => BodyPartApiModel.FromBodyPart(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
