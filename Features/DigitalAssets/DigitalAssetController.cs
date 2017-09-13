using NoDaysOffApp.Features.DigitalAssets.UploadHandlers;

using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http.Headers;

namespace NoDaysOffApp.Features.DigitalAssets
{
    [Authorize]
    [RoutePrefix("api/digitalAsset")]
    public class DigitalAssetController : ApiController
    {        
        public DigitalAssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDigitalAssetCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDigitalAssetCommand.Request request)
            => Ok(await _mediator.Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDigitalAssetCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDigitalAssetCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetsQuery.Response))]
        public async Task<IHttpActionResult> Get()
            => Ok(await _mediator.Send(new GetDigitalAssetsQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDigitalAssetByIdQuery.Request request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDigitalAssetCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDigitalAssetCommand.Request request)
            => Ok(await _mediator.Send(request));

        [Route("serve")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByUniqueIdQuery.Response))]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Serve([FromUri]GetDigitalAssetByUniqueIdQuery.Request request)
        {
            GetDigitalAssetByUniqueIdQuery.Response response = await _mediator.Send(request);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(response.DigitalAsset.Bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(response.DigitalAsset.ContentType);
            return result;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload(HttpRequestMessage request)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());            
            return Ok(await _mediator.Send(new UploadDigitalAssetCommand.Request() { Provider = provider }));
        }

        protected readonly IMediator _mediator;
    }
}