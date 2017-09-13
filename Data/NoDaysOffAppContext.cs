using NoDaysOffApp.Data.Helpers;
using NoDaysOffApp.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace NoDaysOffApp.Data
{
    public interface INoDaysOffAppContext
    {
        DbSet<Athlete> Athletes { get; set; }
        DbSet<Tenant> Tenants { get; set; }
        DbSet<DigitalAsset> DigitalAssets { get; set; }
        DbSet<Dashboard> Dashboards { get; set; }
        DbSet<DashboardTile> DashboardTiles { get; set; }
        DbSet<Tile> Tiles { get; set; }
        DbSet<BodyPart> BodyParts { get; set; }
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Day> Days { get; set; }
        DbSet<ScheduledExercise> ScheduledExercises { get; set; }
        DbSet<CompletedScheduledExercise> CompletedScheduledExercises { get; set; }
        Task<int> SaveChangesAsync();
    }
    
    public class NoDaysOffAppContext : DbContext, INoDaysOffAppContext
    {
        public NoDaysOffAppContext ()
            :base("NoDaysOffAppContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
        }

        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<DigitalAsset> DigitalAssets { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<DashboardTile> DashboardTiles { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<ScheduledExercise> ScheduledExercises { get; set; }
        public DbSet<CompletedScheduledExercise> CompletedScheduledExercises { get; set; }


        public override int SaveChanges()
        {
            UpdateLoggableEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            UpdateLoggableEntries();
            return base.SaveChangesAsync();
        }

        public void UpdateLoggableEntries()
        {
            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
                .Select(x => x.Entity as ILoggable))
            {
                entity.CreatedOn = entity.CreatedOn == default(DateTime) ? DateTime.UtcNow : entity.CreatedOn;
                entity.LastModifiedOn = DateTime.UtcNow;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var convention = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
                "SoftDeleteColumnName",
                (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(convention);
        }
    }
}