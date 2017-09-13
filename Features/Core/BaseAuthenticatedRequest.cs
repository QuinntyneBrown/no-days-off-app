using System;
using System.Linq;
using System.Security.Claims;

namespace NoDaysOffApp.Features.Core
{
    public class BaseAuthenticatedRequest: BaseRequest
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public new Guid TenantUniqueId
        {
            get
            {
                if (ClaimsPrincipal == null)
                    return default(Guid);

                return new Guid(ClaimsPrincipal?.Claims.SingleOrDefault(x => x.Type == Core.ClaimTypes.TenantUniqueId)?.Value);
            }
        }

        public string Username
        {
            get { return ClaimsPrincipal?.Identity.Name; }        
        }
    }
}