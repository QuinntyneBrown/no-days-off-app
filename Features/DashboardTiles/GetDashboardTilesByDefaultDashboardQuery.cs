using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public class GetDashboardTilesByDefaultDashboardQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {
            
        }

        public class Response
        {
            public ICollection<DashboardTileApiModel> DashboardTiles { get; set; } = new HashSet<DashboardTileApiModel>();
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
                var dashboardTiles = await _context.DashboardTiles
                    .Include(x => x.Tenant)
                    .Include(x => x.Tile)
                    .Include(x =>x.Dashboard)
                    .Where(x => x.Dashboard != null
                    && x.Dashboard.Username == request.Username
                    && x.Tenant.UniqueId == request.TenantUniqueId
                    && x.Dashboard.IsDefault)
                    .ToListAsync();

                return new Response()
                {
                    DashboardTiles = dashboardTiles.Select(x => DashboardTileApiModel.FromDashboardTile(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext  _context;
            private readonly ICache _cache;
        }
    }
}
