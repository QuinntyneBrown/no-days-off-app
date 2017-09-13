using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Dashboards
{
    public class GetDashboardsByCurrentUsernameQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {
            
        }

        public class Response
        {
            public ICollection<DashboardApiModel> Dashboards { get; set; } = new HashSet<DashboardApiModel>();
        }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(NoDaysOffAppContext  context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request)
            {
                var dashboards = await _context.Dashboards
                    .Include(x => x.Tenant)
                    .Include(x => x.DashboardTiles)
                    .Include("DashboardTiles.Tile")
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId  && x.Username == request.Username)
                    .ToListAsync();

                return new Response()
                {
                    Dashboards = dashboards.Select(x => DashboardApiModel.FromDashboard(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext  _context;
            private readonly ICache _cache;
        }
    }
}
