using MediatR;
using NoDaysOffApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NoDaysOffApp.Features.DigitalAssets.UploadHandlers;
using System.Collections.Specialized;
using System.Net.Http;
using System.IO;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using static NoDaysOffApp.Features.DigitalAssets.Constants;

namespace NoDaysOffApp.Features.DigitalAssets
{
    public class UploadDigitalAssetCommand
    {
        public class Request : IRequest<Response>
        {
            public InMemoryMultipartFormDataStreamProvider Provider { get; set; }
        }

        public class Response
        {
            public ICollection<DigitalAssetApiModel> DigitalAssets { get; set; }
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
                NameValueCollection formData = request.Provider.FormData;
                IList<HttpContent> files = request.Provider.Files;
                List<DigitalAsset> digitalAssets = new List<DigitalAsset>();
                foreach (var file in files)
                {
                    var filename = new FileInfo(file.Headers.ContentDisposition.FileName.Trim(new char[] { '"' })
                        .Replace("&", "and")).Name;
                    Stream stream = await file.ReadAsStreamAsync();
                    var bytes = StreamHelper.ReadToEnd(stream);
                    var digitalAsset = new DigitalAsset();
                    digitalAsset.FileName = filename;
                    digitalAsset.Bytes = bytes;
                    digitalAsset.ContentType = System.Convert.ToString(file.Headers.ContentType);
                    _context.DigitalAssets.Add(digitalAsset);
                    digitalAssets.Add(digitalAsset);
                }

                await _context.SaveChangesAsync();

                _cache.Add(null, DigitalAssetCacheKeys.DigitalAssets);

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
