using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Messages
{
    public class AddOrUpdateMessageCommand
    {
        public class Request : BaseRequest, IRequest<Response>
        {
            public MessageApiModel Message { get; set; }            
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
                var entity = await _context.Messages
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Message.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Messages.Add(entity = new Message() { TenantId = tenant.Id });
                }
                
                await _context.SaveChangesAsync();

                _bus.Publish(new AddedOrUpdatedMessageMessage(entity, request.CorrelationId, request.TenantUniqueId));

                return new Response();
            }

            private readonly NoDaysOffAppContext _context;
            private readonly IEventBus _bus;
        }
    }
}
