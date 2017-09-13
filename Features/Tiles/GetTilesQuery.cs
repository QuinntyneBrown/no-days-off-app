using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Tiles
{
    public class GetTilesQuery
    {
        public class Request : BaseRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<TileApiModel> Tiles { get; set; } = new HashSet<TileApiModel>();
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
                var tiles = await _context.Tiles
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new Response()
                {
                    Tiles = tiles.Select(x => TileApiModel.FromTile(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext  _context;
            private readonly ICache _cache;
        }
    }
}
