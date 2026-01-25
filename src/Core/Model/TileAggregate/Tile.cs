using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.TileAggregate;

/// <summary>
/// Aggregate root for tile definitions used in dashboards
/// </summary>
public sealed class Tile : AggregateRoot
{
    public const int MaxNameLength = 256;

    public string Name { get; private set; } = string.Empty;
    public int DefaultHeight { get; private set; }
    public int DefaultWidth { get; private set; }
    public bool IsVisibleInCatalog { get; private set; }

    private Tile() : base()
    {
    }

    private Tile(int? tenantId) : base(tenantId)
    {
    }

    public static Tile Create(int? tenantId, string name, int defaultWidth, int defaultHeight, string createdBy)
    {
        ValidateName(name);
        ValidateDimensions(defaultWidth, defaultHeight);

        var tile = new Tile(tenantId)
        {
            Name = name,
            DefaultWidth = defaultWidth,
            DefaultHeight = defaultHeight,
            IsVisibleInCatalog = true
        };
        tile.SetAuditInfo(createdBy);

        return tile;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void UpdateDefaultDimensions(int defaultWidth, int defaultHeight, string modifiedBy)
    {
        ValidateDimensions(defaultWidth, defaultHeight);
        DefaultWidth = defaultWidth;
        DefaultHeight = defaultHeight;
        UpdateModified(modifiedBy);
    }

    public void ShowInCatalog(string modifiedBy)
    {
        IsVisibleInCatalog = true;
        UpdateModified(modifiedBy);
    }

    public void HideFromCatalog(string modifiedBy)
    {
        IsVisibleInCatalog = false;
        UpdateModified(modifiedBy);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tile name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Tile name cannot exceed {MaxNameLength} characters");
        }
    }

    private static void ValidateDimensions(int width, int height)
    {
        if (width <= 0)
        {
            throw new ValidationException("Default width must be greater than zero");
        }

        if (height <= 0)
        {
            throw new ValidationException("Default height must be greater than zero");
        }
    }
}
