using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.DashboardAggregate;

/// <summary>
/// Aggregate root for dashboard management with tiles
/// </summary>
public sealed class Dashboard : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<DashboardTile> _tiles = new();

    public string Name { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }

    public IReadOnlyCollection<DashboardTile> Tiles => _tiles.AsReadOnly();

    private Dashboard() : base()
    {
    }

    private Dashboard(int? tenantId) : base(tenantId)
    {
    }

    public static Dashboard Create(int? tenantId, string name, string username, bool isDefault, string createdBy)
    {
        ValidateName(name);

        var dashboard = new Dashboard(tenantId)
        {
            Name = name,
            Username = username,
            IsDefault = isDefault
        };
        dashboard.SetAuditInfo(createdBy);

        return dashboard;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void SetAsDefault(string modifiedBy)
    {
        IsDefault = true;
        UpdateModified(modifiedBy);
    }

    public void ClearDefault(string modifiedBy)
    {
        IsDefault = false;
        UpdateModified(modifiedBy);
    }

    public void AddTile(int tileId, string name, int top, int left, int width, int height, string modifiedBy)
    {
        var tile = DashboardTile.Create(tileId, name, top, left, width, height);
        _tiles.Add(tile);
        UpdateModified(modifiedBy);
    }

    public void RemoveTile(int index, string modifiedBy)
    {
        if (index >= 0 && index < _tiles.Count)
        {
            _tiles.RemoveAt(index);
            UpdateModified(modifiedBy);
        }
    }

    public void UpdateTilePosition(int index, int top, int left, string modifiedBy)
    {
        if (index >= 0 && index < _tiles.Count)
        {
            _tiles[index].UpdatePosition(top, left);
            UpdateModified(modifiedBy);
        }
    }

    public void UpdateTileSize(int index, int width, int height, string modifiedBy)
    {
        if (index >= 0 && index < _tiles.Count)
        {
            _tiles[index].UpdateSize(width, height);
            UpdateModified(modifiedBy);
        }
    }

    public void ClearTiles(string modifiedBy)
    {
        if (_tiles.Count > 0)
        {
            _tiles.Clear();
            UpdateModified(modifiedBy);
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Dashboard name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Dashboard name cannot exceed {MaxNameLength} characters");
        }
    }
}
