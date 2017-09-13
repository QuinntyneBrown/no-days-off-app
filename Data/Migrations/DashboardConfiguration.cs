using System.Data.Entity.Migrations;
using NoDaysOffApp.Model;
using System.Linq;

namespace NoDaysOffApp.Data.Migrations
{
    public class DashboardConfiguration
    {
        public static void Seed(NoDaysOffAppContext  context)
        {
            var tenant = context.Tenants.First();

            context.Dashboards.AddOrUpdate(x => x.Name, new Dashboard()
            {
                Name = "Home",
                IsDefault = true,
                Username = "quinntynebrown@gmail.com",
                TenantId = tenant.Id
            });

            context.SaveChanges();
        }
    }
}