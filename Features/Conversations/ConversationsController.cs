using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Conversations
{
    [Authorize]
    [RoutePrefix("api/conversations")]
    public class ConversationController : BaseApiController
    {
        public ConversationController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateConversationCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateConversationCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateConversationCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateConversationCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetConversationsQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetConversationsQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetConversationByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetConversationByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveConversationCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveConversationCommand.Request request) => Ok(await Send(request));

    }
}
