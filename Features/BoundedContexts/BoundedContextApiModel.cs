using NoDaysOffApp.Features.Core;
using NoDaysOffApp.Model;
using static NoDaysOffApp.Features.Core.NamingConventionConverter;

namespace NoDaysOffApp.Features.BoundedContexts
{
    public class BoundedContextApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string NameCamelCase {  get { return Convert(NamingConvention.CamelCase, Name); } }
        public string NameTitleCase {  get { return Convert(NamingConvention.TitleCase, Name); } }

        public static TModel FromBoundedContext<TModel>(BoundedContext boundedContext) where
            TModel : BoundedContextApiModel, new()
        {
            var model = new TModel();
            model.Id = boundedContext.Id;
            model.TenantId = boundedContext.TenantId;
            model.Name = boundedContext.Name;
            model.ImageUrl = boundedContext.ImageUrl;
            model.Description = boundedContext.Description;
            return model;
        }

        public static BoundedContextApiModel FromBoundedContext(BoundedContext boundedContext)
            => FromBoundedContext<BoundedContextApiModel>(boundedContext);

    }
}
