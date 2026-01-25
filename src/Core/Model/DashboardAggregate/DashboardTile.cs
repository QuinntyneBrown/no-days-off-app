using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.DashboardAggregate;

/// <summary>
/// Entity representing a tile on a dashboard with position and size
/// </summary>
public sealed class DashboardTile : Entity
{
    public const int MaxNameLength = 256;

    public string Name { get; private set; } = string.Empty;
    public int TileId { get; private set; }
    public int Top { get; private set; }
    public int Left { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private DashboardTile()
    {
    }

    internal static DashboardTile Create(int tileId, string name, int top, int left, int width, int height)
    {
        ValidateName(name);
        ValidateDimensions(width, height);

        return new DashboardTile
        {
            TileId = tileId,
            Name = name,
            Top = top,
            Left = left,
            Width = width,
            Height = height
        };
    }

    internal void UpdatePosition(int top, int left)
    {
        Top = top;
        Left = left;
    }

    internal void UpdateSize(int width, int height)
    {
        ValidateDimensions(width, height);
        Width = width;
        Height = height;
    }

    internal void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Dashboard tile name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Dashboard tile name cannot exceed {MaxNameLength} characters");
        }
    }

    private static void ValidateDimensions(int width, int height)
    {
        if (width <= 0)
        {
            throw new ValidationException("Width must be greater than zero");
        }

        if (height <= 0)
        {
            throw new ValidationException("Height must be greater than zero");
        }
    }
}
