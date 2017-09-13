using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using System.Threading.Tasks;
using System.Data.Entity;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class AddOrUpdateDigitalAssetCommand
    {
        public class Request : IRequest<Response>
        {
            public DigitalAssetApiModel DigitalAsset { get; set; }
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
                var entity = await _context.DigitalAssets
                    .SingleOrDefaultAsync(x => x.Id == request.DigitalAsset.Id && x.IsDeleted == false);
                if (entity == null) _context.DigitalAssets.Add(entity = new DigitalAsset());
                entity.Name = request.DigitalAsset.Name;
                entity.Folder = request.DigitalAsset.Folder;
                await _context.SaveChangesAsync();

                return new Response() { };
            }

            private readonly INoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
