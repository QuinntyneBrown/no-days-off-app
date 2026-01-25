using Shared.Domain;

namespace Dashboard.Core.Aggregates.Widget;

public class Widget : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public WidgetType Type { get; private set; }
    public int TenantId { get; private set; }
    public int UserId { get; private set; }
    public int Position { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public string? Configuration { get; private set; }
    public bool IsVisible { get; private set; }

    private Widget() { }

    public Widget(
        string name,
        WidgetType type,
        int tenantId,
        int userId,
        int position = 0,
        int width = 1,
        int height = 1,
        string? configuration = null)
    {
        Name = name;
        Type = type;
        TenantId = tenantId;
        UserId = userId;
        Position = position;
        Width = width;
        Height = height;
        Configuration = configuration;
        IsVisible = true;
    }

    public void UpdatePosition(int position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }

    public void UpdateConfiguration(string? configuration)
    {
        Configuration = configuration;
    }

    public void SetVisibility(bool isVisible)
    {
        IsVisible = isVisible;
    }
}

public enum WidgetType
{
    WorkoutSummary = 0,
    AthleteProgress = 1,
    RecentActivity = 2,
    UpcomingWorkouts = 3,
    Statistics = 4,
    Goals = 5,
    Calendar = 6
}
