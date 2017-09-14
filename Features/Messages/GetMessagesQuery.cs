using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Messages
{
    public class GetMessagesQuery
    {
        public class Request : BaseRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<MessageApiModel> Messages { get; set; } = new HashSet<MessageApiModel>();
        }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public Handler(NoDaysOffAppContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request)
            {
                var messages = await _context.Messages
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new Response()
                {
                    Messages = messages.Select(x => MessageApiModel.FromMessage(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
