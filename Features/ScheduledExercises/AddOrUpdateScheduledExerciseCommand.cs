using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.ScheduledExercises
{
    public class AddOrUpdateScheduledExerciseCommand
    {
        public class Request : BaseRequest, IRequest<Response>
        {
            public ScheduledExerciseApiModel ScheduledExercise { get; set; }            
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
                var entity = await _context.ScheduledExercises
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.ScheduledExercise.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.ScheduledExercises.Add(entity = new ScheduledExercise() { TenantId = tenant.Id });
                }

                entity.Name = request.ScheduledExercise.Name;
                
                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedScheduledExerciseMessage(entity, request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext _context;
            private readonly IEventBus _bus;
        }
    }
}
