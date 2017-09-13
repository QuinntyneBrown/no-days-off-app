using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.BodyParts
{
    [Authorize]
    [RoutePrefix("api/bodyParts")]
    public class BodyPartController : BaseApiController
    {
        public BodyPartController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateBodyPartCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateBodyPartCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateBodyPartCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateBodyPartCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetBodyPartsQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetBodyPartsQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetBodyPartByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetBodyPartByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveBodyPartCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveBodyPartCommand.Request request) => Ok(await Send(request));

    }
}
