using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Tiles
{
    [Authorize]
    [RoutePrefix("api/tiles")]
    public class TileController : BaseApiController
    {
        public TileController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateTileCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateTileCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateTileCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateTileCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetTilesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetTilesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetTileByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetTileByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveTileCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveTileCommand.Request request) => Ok(await Send(request));

    }
}
