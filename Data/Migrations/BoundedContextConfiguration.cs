using System.Data.Entity.Migrations;
using NoDaysOffApp.Data;
using System.Linq;
using NoDaysOffApp.Model;

namespace NoDaysOffApp.Data.Migrations
{
    public class BoundedContextConfiguration
    {
        public static void Seed(NoDaysOffAppContext context) {

            var tenant = context.Tenants.First();

            context.BoundedContexts.AddOrUpdate(x => x.Name, new BoundedContext()
            {
                Name = "AthleteWeights",
                TenantId = tenant.Id
            });

            context.BoundedContexts.AddOrUpdate(x => x.Name, new BoundedContext()
            {
                Name = "Exercises",
                TenantId = tenant.Id
            });

            context.BoundedContexts.AddOrUpdate(x => x.Name, new BoundedContext()
            {
                Name = "Videos",
                TenantId = tenant.Id
            });

            context.BoundedContexts.AddOrUpdate(x => x.Name, new BoundedContext()
            {
                Name = "BodyParts",
                TenantId = tenant.Id
            });

            context.BoundedContexts.AddOrUpdate(x => x.Name, new BoundedContext()
            {
                Name = "ScheduledExercises",
                TenantId = tenant.Id
            });

            context.SaveChanges();
        }
    }
}
