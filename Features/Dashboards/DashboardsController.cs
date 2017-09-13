using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Dashboards
{
    [Authorize]
    [RoutePrefix("api/dashboards")]
    public class DashboardController : BaseApiController
    {
        public DashboardController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDashboardCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDashboardCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDashboardCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDashboardCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardsQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetDashboardsQuery.Request()));

        [Route("getbycurrentusername")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardsByCurrentUsernameQuery.Response))]
        public async Task<IHttpActionResult> GetByCurrentUsername() => Ok(await Send(new GetDashboardsByCurrentUsernameQuery.Request()));

        [Route("getdefault")]
        [HttpGet]
        [ResponseType(typeof(GetDefaultDashboardQuery.Response))]
        public async Task<IHttpActionResult> GetDefault() => Ok(await Send(new GetDefaultDashboardQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDashboardByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDashboardCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDashboardCommand.Request request) => Ok(await Send(request));

    }
}
