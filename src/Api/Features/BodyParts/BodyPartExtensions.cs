using Core.Model.BodyPartAggregate;

namespace Api;

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
