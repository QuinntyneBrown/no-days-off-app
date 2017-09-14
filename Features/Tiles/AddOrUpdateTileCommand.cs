using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Tiles
{
    public class AddOrUpdateTileCommand
    {
        public class Request : BaseRequest, IRequest<Response>
        {
            public TileApiModel Tile { get; set; }            
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
                var entity = await _context.Tiles
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Tile.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Tiles.Add(entity = new Tile() { TenantId = tenant.Id });
                }

                entity.Name = request.Tile.Name;

                entity.IsVisibleInCatalog = request.Tile.IsVisibleInCatalog;
                
                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedTileMessage(entity, request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext  _context;
            private readonly IEventBus _bus;
        }
    }
}
