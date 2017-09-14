namespace NoDaysOffApp.Migrations
{
    using NoDaysOffApp.Data.Migrations;
    using Data;
    using Data.Helpers;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<NoDaysOffAppContext >
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NoDaysOffAppContext  context)
        {
            
            TenantConfiguration.Seed(context);
            DashboardConfiguration.Seed(context);
            DayConfiguration.Seed(context);
            TileConfiguration.Seed(context);
        }
    }

    public class DbConfiguration : System.Data.Entity.DbConfiguration
    {
        public DbConfiguration()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}
