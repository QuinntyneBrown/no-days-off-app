namespace Shared.Contracts.Dashboard;

public class WidgetDto
{
    public int WidgetId { get; set; }
    public string Name { get; set; } = null!;
    public int Type { get; set; }
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public int Position { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string? Configuration { get; set; }
    public bool IsVisible { get; set; }
}

public class CreateWidgetDto
{
    public string Name { get; set; } = null!;
    public int Type { get; set; }
    public int Position { get; set; }
    public int Width { get; set; } = 1;
    public int Height { get; set; } = 1;
    public string? Configuration { get; set; }
}

public class DashboardStatsDto
{
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public int TotalWorkouts { get; set; }
    public int TotalExercises { get; set; }
    public int TotalAthletes { get; set; }
    public int WorkoutsThisWeek { get; set; }
    public int WorkoutsThisMonth { get; set; }
    public DateTime LastUpdated { get; set; }
}
