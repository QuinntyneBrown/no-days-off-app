using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.BoundedContexts
{
    public class AddOrUpdateBoundedContextCommand
    {
        public class Request : BaseRequest, IRequest<Response>
        {
            public BoundedContextApiModel BoundedContext { get; set; }            
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
                var entity = await _context.BoundedContexts
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.BoundedContext.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.BoundedContexts.Add(entity = new BoundedContext() { TenantId = tenant.Id });
                }

                entity.Name = request.BoundedContext.Name;

                entity.Description = request.BoundedContext.Description;

                entity.ImageUrl = request.BoundedContext.ImageUrl;
                
                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedBoundedContextMessage(entity, request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext _context;
            private readonly IEventBus _bus;
        }
    }
}
