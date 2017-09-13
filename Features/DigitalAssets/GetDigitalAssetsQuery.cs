using MediatR;
using NoDaysOffApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using NoDaysOffApp.Model;
using static NoDaysOffApp.Features.DigitalAssets.Constants;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class GetDigitalAssetsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public ICollection<DigitalAssetApiModel> DigitalAssets { get; set; } = new HashSet<DigitalAssetApiModel>();
        }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(INoDaysOffAppContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request)
            {
                var digitalAssets = await _cache.FromCacheOrServiceAsync<List<DigitalAsset>>(() => _context.DigitalAssets.ToListAsync(), DigitalAssetCacheKeys.DigitalAssets);

                return new Response()
                {
                    DigitalAssets = digitalAssets.Select(x => DigitalAssetApiModel.FromDigitalAsset(x)).ToList()
                };
            }

            private readonly INoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}