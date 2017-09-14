using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Profiles
{
    public class GetProfilesQuery
    {
        public class Request : BaseRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<ProfileApiModel> Profiles { get; set; } = new HashSet<ProfileApiModel>();
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
                var profiles = await _context.Profiles
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new Response()
                {
                    Profiles = profiles.Select(x => ProfileApiModel.FromProfile(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
