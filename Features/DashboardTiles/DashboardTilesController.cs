using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.DashboardTiles
{
    [Authorize]
    [RoutePrefix("api/dashboardTiles")]
    public class DashboardTileController : BaseApiController
    {
        public DashboardTileController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDashboardTileCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDashboardTileCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDashboardTileCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDashboardTileCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetDashboardTilesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetDashboardTilesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardTileByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDashboardTileByIdQuery.Request request) => Ok(await Send(request));

        [Route("getByDashboardId")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardTilesByDashboardIdQuery.Response))]
        public async Task<IHttpActionResult> GetByDashboardId([FromUri]GetDashboardTilesByDashboardIdQuery.Request request) => Ok(await Send(request));

        [Route("getByDefaultDashboard")]
        [HttpGet]
        [ResponseType(typeof(GetDashboardTilesByDefaultDashboardQuery.Response))]
        public async Task<IHttpActionResult> GetByDefaultDashboard() => Ok(await Send(new GetDashboardTilesByDefaultDashboardQuery.Request()));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDashboardTileCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDashboardTileCommand.Request request) => Ok(await Send(request));

    }
}
