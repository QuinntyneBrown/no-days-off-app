using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Exercises
{
    [Authorize]
    [RoutePrefix("api/exercises")]
    public class ExerciseController : BaseApiController
    {
        public ExerciseController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateExerciseCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateExerciseCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateExerciseCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateExerciseCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetExercisesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetExercisesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetExerciseByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetExerciseByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveExerciseCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveExerciseCommand.Request request) => Ok(await Send(request));

    }
}
