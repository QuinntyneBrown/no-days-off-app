using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{
    [Authorize]
    [RoutePrefix("api/completedScheduledExercises")]
    public class CompletedScheduledExerciseController : BaseApiController
    {
        public CompletedScheduledExerciseController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateCompletedScheduledExerciseCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateCompletedScheduledExerciseCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateCompletedScheduledExerciseCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateCompletedScheduledExerciseCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetCompletedScheduledExercisesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetCompletedScheduledExercisesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetCompletedScheduledExerciseByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetCompletedScheduledExerciseByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveCompletedScheduledExerciseCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveCompletedScheduledExerciseCommand.Request request) => Ok(await Send(request));

    }
}
