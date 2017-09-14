using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Athletes
{
    public class AddOrUpdateAthleteCommand
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {
            public AthleteApiModel Athlete { get; set; }            
			public Guid CorrelationId { get; set; }
        }

        public class Response { }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(NoDaysOffAppContext context, IEventBus bus)
            {
                _context = context;
                _bus = bus;
            }

            public async Task<Response> Handle(Request request)
            {                
                var entity = await _context.Athletes
                    .Include(x => x.Tenant)
                    .Include(x=>x.AthleteWeights)
                    .Include(x=>x.CompletedScheduledExercises)
                    .SingleOrDefaultAsync(x => x.Id == request.Athlete.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {

                    if (this._context.Athletes.SingleOrDefault(x => x.Username == request.Username) != null)
                        throw new Exception("Only one Athlete per logged in User");

                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Athletes.Add(entity = new Athlete() { TenantId = tenant.Id });
                }

                entity.Name = request.Athlete.Name;

                entity.Username = request.Username;

                entity.ImageUrl = request.Athlete.ImageUrl;

                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedAthleteMessage(entity, request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext _context;
            private readonly IEventBus _bus;
        }
    }
}
