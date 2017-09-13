using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public class AddOrUpdateDashboardTileCommand
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {
            public DashboardTileApiModel DashboardTile { get; set; }            
			public Guid CorrelationId { get; set; }
        }

        public class Response { }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(NoDaysOffAppContext  context, IEventBus bus)
            {
                _context = context;
                _bus = bus;
            }

            public async Task<Response> Handle(Request request)
            {
                var entity = await _context.DashboardTiles
                    .Include(x => x.Tenant)
                    .Include(x => x.Dashboard)
                    .Include(x => x.Tile)
                    .SingleOrDefaultAsync(x => x.Id == request.DashboardTile.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    var dashboard = await _context.Dashboards.Include(x=>x.DashboardTiles).SingleAsync(x => x.Id == request.DashboardTile.DashboardId);
                    var tile = await _context.Tiles.SingleAsync(x => x.Id == request.DashboardTile.TileId);

                    if (dashboard.DashboardTiles.SingleOrDefault(x => x.TileId == request.DashboardTile.TileId) != null)
                        throw new Exception("Tile Exists. Can not add duplicate");

                    _context.DashboardTiles.Add(entity = new DashboardTile() { TenantId = tenant.Id, Dashboard = dashboard, Tile = tile });
                }
                
                entity.Name = request.DashboardTile.Name;
                entity.DashboardId = request.DashboardTile.DashboardId;
                entity.TileId = request.DashboardTile.TileId;
                entity.Top = request.DashboardTile.Top;
                entity.Left = request.DashboardTile.Left;
                entity.Width = request.DashboardTile.Width;
                entity.Height = request.DashboardTile.Height;
                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedDashboardTileMessage(DashboardTileApiModel.FromDashboardTile(entity), request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext  _context;
            private readonly IEventBus _bus;
        }
    }
}
