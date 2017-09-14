using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Videos
{
    [Authorize]
    [RoutePrefix("api/videos")]
    public class VideoController : BaseApiController
    {
        public VideoController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateVideoCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateVideoCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateVideoCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateVideoCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetVideosQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetVideosQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetVideoByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetVideoByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveVideoCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveVideoCommand.Request request) => Ok(await Send(request));

    }
}
