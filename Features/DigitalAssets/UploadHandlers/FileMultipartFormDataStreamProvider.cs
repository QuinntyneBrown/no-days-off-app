using System.Net.Http;

namespace NoDaysOffApp.Features.DigitalAssets.UploadHandlers
{
    public class FileMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public FileMultipartFormDataStreamProvider(string path)
            : base(path) { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
            => (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName")
            .Trim(new char[] { '"' })
            .Replace("&", "and");        
    }
}
