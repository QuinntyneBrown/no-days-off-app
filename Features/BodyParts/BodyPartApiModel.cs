using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.BodyParts
{
    public class BodyPartApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromBodyPart<TModel>(BodyPart bodyPart) where
            TModel : BodyPartApiModel, new()
        {
            var model = new TModel();
            model.Id = bodyPart.Id;
            model.TenantId = bodyPart.TenantId;
            model.Name = bodyPart.Name;
            return model;
        }

        public static BodyPartApiModel FromBodyPart(BodyPart bodyPart)
            => FromBodyPart<BodyPartApiModel>(bodyPart);

    }
}
