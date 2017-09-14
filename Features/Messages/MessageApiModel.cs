using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Messages
{
    public class MessageApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }

        public static TModel FromMessage<TModel>(Message message) where
            TModel : MessageApiModel, new()
        {
            var model = new TModel();
            model.Id = message.Id;
            model.TenantId = message.TenantId;            
            return model;
        }

        public static MessageApiModel FromMessage(Message message)
            => FromMessage<MessageApiModel>(message);

    }
}
