using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Days
{
    [Authorize]
    [RoutePrefix("api/days")]
    public class DayController : BaseApiController
    {
        public DayController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDayCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDayCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDayCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDayCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetDaysQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetDaysQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDayByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDayByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDayCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDayCommand.Request request) => Ok(await Send(request));

    }
}
