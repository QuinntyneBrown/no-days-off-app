using MediatR;
using NoDaysOffApp.Data;
using System.Threading.Tasks;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class RemoveDigitalAssetCommand
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response { }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(INoDaysOffAppContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request)
            {
                var digitalAsset = await _context.DigitalAssets.FindAsync(request.Id);
                digitalAsset.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new Response();
            }

            private readonly INoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
