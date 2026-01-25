using MessagePack;

namespace Shared.Messaging.Messages.Media;

[MessagePackObject]
public record MediaUploadedMessage(
    [property: Key(0)] int MediaId,
    [property: Key(1)] string FileName,
    [property: Key(2)] int TenantId);

[MessagePackObject]
public record MediaDeletedMessage(
    [property: Key(0)] int MediaId,
    [property: Key(1)] int TenantId);
