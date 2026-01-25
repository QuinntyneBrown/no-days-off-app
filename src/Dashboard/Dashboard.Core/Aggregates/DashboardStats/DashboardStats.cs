using Shared.Domain;

namespace Dashboard.Core.Aggregates.DashboardStats;

public class DashboardStats : AggregateRoot
{
    public int TenantId { get; private set; }
    public int UserId { get; private set; }
    public int TotalWorkouts { get; private set; }
    public int TotalExercises { get; private set; }
    public int TotalAthletes { get; private set; }
    public int WorkoutsThisWeek { get; private set; }
    public int WorkoutsThisMonth { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private DashboardStats() { }

    public DashboardStats(int tenantId, int userId)
    {
        TenantId = tenantId;
        UserId = userId;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateStats(
        int totalWorkouts,
        int totalExercises,
        int totalAthletes,
        int workoutsThisWeek,
        int workoutsThisMonth)
    {
        TotalWorkouts = totalWorkouts;
        TotalExercises = totalExercises;
        TotalAthletes = totalAthletes;
        WorkoutsThisWeek = workoutsThisWeek;
        WorkoutsThisMonth = workoutsThisMonth;
        LastUpdated = DateTime.UtcNow;
    }

    public void IncrementWorkouts()
    {
        TotalWorkouts++;
        WorkoutsThisWeek++;
        WorkoutsThisMonth++;
        LastUpdated = DateTime.UtcNow;
    }

    public void IncrementExercises()
    {
        TotalExercises++;
        LastUpdated = DateTime.UtcNow;
    }

    public void IncrementAthletes()
    {
        TotalAthletes++;
        LastUpdated = DateTime.UtcNow;
    }
}
