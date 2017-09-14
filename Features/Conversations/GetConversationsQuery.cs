using MediatR;
using NoDaysOffApp.Data;
using NoDaysOffApp.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace NoDaysOffApp.Features.Conversations
{
    public class GetConversationsQuery
    {
        public class Request : BaseRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<ConversationApiModel> Conversations { get; set; } = new HashSet<ConversationApiModel>();
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
                var conversations = await _context.Conversations
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new Response()
                {
                    Conversations = conversations.Select(x => ConversationApiModel.FromConversation(x)).ToList()
                };
            }

            private readonly NoDaysOffAppContext _context;
            private readonly ICache _cache;
        }
    }
}
