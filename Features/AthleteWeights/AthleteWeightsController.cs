using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.AthleteWeights
{
    [Authorize]
    [RoutePrefix("api/athleteWeights")]
    public class AthleteWeightController : BaseApiController
    {
        public AthleteWeightController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateAthleteWeightCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateAthleteWeightCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateAthleteWeightCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateAthleteWeightCommand.Request request) => Ok(await Send(request));

        [Route("get")]
        [HttpGet]
        [ResponseType(typeof(GetAthleteWeightsQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetAthleteWeightsQuery.Request()));

        [Route("getCurrent")]
        [HttpGet]
        [ResponseType(typeof(GetCurrentAthleteWeightQuery.Response))]
        public async Task<IHttpActionResult> GetCurrent() => Ok(await Send(new GetCurrentAthleteWeightQuery.Request()));


        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetAthleteWeightByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetAthleteWeightByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveAthleteWeightCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveAthleteWeightCommand.Request request) => Ok(await Send(request));

    }
}
