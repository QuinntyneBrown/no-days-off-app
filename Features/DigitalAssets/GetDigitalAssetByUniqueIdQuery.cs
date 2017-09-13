using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class GetDigitalAssetByUniqueIdQuery
    {
        public class Request : IRequest<Response>
        {
            public string UniqueId { get; set; }
        }

        public class Response
        {
            public DigitalAssetApiModel DigitalAsset { get; set; }
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
                    DigitalAsset = DigitalAssetApiModel.FromDigitalAsset(await _context.DigitalAssets.SingleAsync(x=>x.UniqueId.ToString() == request.UniqueId))
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }

    }

}
