using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NoDaysOffApp.Features.Core;

namespace NoDaysOffApp.Features.Profiles
{
    [Authorize]
    [RoutePrefix("api/profiles")]
    public class ProfileController : BaseApiController
    {
        public ProfileController(IMediator mediator)
            :base(mediator) { }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateProfileCommand.Response))]
        public async Task<IHttpActionResult> Add(AddOrUpdateProfileCommand.Request request) => Ok(await Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateProfileCommand.Response))]
        public async Task<IHttpActionResult> Update(AddOrUpdateProfileCommand.Request request) => Ok(await Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetProfilesQuery.Response))]
        public async Task<IHttpActionResult> Get() => Ok(await Send(new GetProfilesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetProfileByIdQuery.Response))]
        public async Task<IHttpActionResult> GetById([FromUri]GetProfileByIdQuery.Request request) => Ok(await Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveProfileCommand.Response))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveProfileCommand.Request request) => Ok(await Send(request));

    }
}
