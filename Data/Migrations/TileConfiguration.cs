using System.Data.Entity.Migrations;
using NoDaysOffApp.Model;
using System.Linq;

namespace NoDaysOffApp.Data.Migrations
{
    public class TileConfiguration
    {
        public static void Seed(NoDaysOffAppContext context) {

            var tenant = context.Tenants.First();

            context.Tiles.AddOrUpdate(x => x.Name, new Tile()
            {
                Name = "Athlete Weight",
                TenantId = tenant.Id
            });

            context.SaveChanges();
        }
    }
}
