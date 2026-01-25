namespace Api;

public sealed record TileDto(
    int TileId,
    string Name,
    int DefaultHeight,
    int DefaultWidth,
    bool IsVisibleInCatalog,
    DateTime CreatedOn,
    string CreatedBy);
