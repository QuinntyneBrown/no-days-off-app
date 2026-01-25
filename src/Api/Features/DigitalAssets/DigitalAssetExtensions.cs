using Core.Model.DigitalAssetAggregate;

namespace Api;

public static class DigitalAssetExtensions
{
    public static DigitalAssetDto ToDto(this DigitalAsset digitalAsset)
    {
        return new DigitalAssetDto(
            digitalAsset.Id,
            digitalAsset.Name,
            digitalAsset.FileName,
            digitalAsset.Description,
            digitalAsset.Folder,
            digitalAsset.ContentType,
            digitalAsset.Size,
            digitalAsset.UniqueId,
            digitalAsset.RelativePath,
            digitalAsset.CreatedOn,
            digitalAsset.CreatedBy);
    }
}
