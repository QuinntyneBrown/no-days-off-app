using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Conversations
{
    public class ConversationApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        
        public static TModel FromConversation<TModel>(Conversation conversation) where
            TModel : ConversationApiModel, new()
        {
            var model = new TModel();
            model.Id = conversation.Id;
            model.TenantId = conversation.TenantId;            
            return model;
        }

        public static ConversationApiModel FromConversation(Conversation conversation)
            => FromConversation<ConversationApiModel>(conversation);

    }
}
