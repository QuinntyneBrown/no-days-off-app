using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.BoundedContexts
{
    [Authorize]
    [RoutePrefix("api/boundedContexts")]
    public class BoundedContextController : BaseApiController
    {
        public BoundedContextController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateBoundedContextCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateBoundedContextCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateBoundedContextCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateBoundedContextCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetBoundedContextsQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetBoundedContextsQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetBoundedContextByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetBoundedContextByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveBoundedContextCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveBoundedContextCommand.Request request) => Ok(await Send(request));

    }
}
