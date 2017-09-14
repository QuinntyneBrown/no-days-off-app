using System;
using System.Collections.Generic;
using NoDaysOffApp.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static NoDaysOffApp.Constants;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class Message: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
        public int FromId { get; set; }

        public int ToId { get; set; }

        public string Body { get; set; }

        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
