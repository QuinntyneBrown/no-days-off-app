using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Messages
{
    [Authorize]
    [RoutePrefix("api/messages")]
    public class MessageController : BaseApiController
    {
        public MessageController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateMessageCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateMessageCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateMessageCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateMessageCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetMessagesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetMessagesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetMessageByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetMessageByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveMessageCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveMessageCommand.Request request) => Ok(await Send(request));

    }
}
