using MediatR;
using NoDaysOffApp.Data;
using System.Threading.Tasks;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class GetDigitalAssetByIdQuery
    {
        public class Request : IRequest<Response> { 
			public int Id { get; set; }
		}

        public class Response
        {
            public DigitalAssetApiModel DigitalAsset { get; set; } 
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
                return new Response()
                {
                    DigitalAsset = DigitalAssetApiModel.FromDigitalAsset(await _context.DigitalAssets.FindAsync(request.Id))
                };
            }

            private readonly INoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
