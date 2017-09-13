using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Athletes
{
    [Authorize]
    [RoutePrefix("api/athletes")]
    public class AthleteController : BaseApiController
    {
        public AthleteController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateAthleteCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateAthleteCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateAthleteCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateAthleteCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [HttpGet]
        [ResponseType(typeof(GetAthletesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetAthletesQuery.Request()));

        [Route("getcurrent")]
        [HttpGet]
        [ResponseType(typeof(GetCurrentAthleteQuery.Response))]
        public async Task<IHttpActionResult> GetCurrent() => Ok(await Send(new GetCurrentAthleteQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetAthleteByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetAthleteByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveAthleteCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveAthleteCommand.Request request) => Ok(await Send(request));

    }
}
