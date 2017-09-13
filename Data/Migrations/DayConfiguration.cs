using System.Data.Entity.Migrations;
using NoDaysOffApp.Model;
using System.Linq;

namespace NoDaysOffApp.Data.Migrations
{
    public class DayConfiguration
    {
        public static void Seed(NoDaysOffAppContext context) {

            var tenant = context.Tenants.First();

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Monday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Tuesday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Wednesday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Thursday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Friday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Saturday",
                TenantId = tenant.Id
            });

            context.Days.AddOrUpdate(x => x.Name, new Day()
            {
                Name = "Sunday",
                TenantId = tenant.Id
            });
            context.SaveChanges();
        }
    }
}
