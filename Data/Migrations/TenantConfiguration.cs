using NoDaysOffApp.Data;
using NoDaysOffApp.Model;
using System.Data.Entity.Migrations;
using System;

namespace NoDaysOffApp.Migrations
{
    public class TenantConfiguration
    {
        public static void Seed(NoDaysOffAppContext  context) {

            context.Tenants.AddOrUpdate(x => x.Name, new Tenant()
            {
                Name = "Default",
                UniqueId = new Guid("af3552b9-c3cd-4cea-b239-b1a31f887c23")
            });

            context.SaveChanges();
        }
    }
}
