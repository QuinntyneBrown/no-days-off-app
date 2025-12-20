using NoDaysOff.Core.Model.BodyPartAggregate;

namespace NoDaysOff.Api;

public static class BodyPartExtensions
{
    public static BodyPartDto ToDto(this BodyPart bodyPart)
    {
        return new BodyPartDto(
            bodyPart.Id,
            bodyPart.Name,
            bodyPart.CreatedOn,
            bodyPart.CreatedBy);
    }
}
