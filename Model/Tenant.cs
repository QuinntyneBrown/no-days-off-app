using NoDaysOffApp.Data.Helpers;
using static NoDaysOffApp.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoDaysOffApp.Model
{
    [SoftDelete("IsDeleted")]
    public class Tenant: ILoggable
    {
        public int Id { get; set; }

        [Index("UniqueIdIndex", IsUnique = true)]
        [Column(TypeName = "UNIQUEIDENTIFIER")]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Index("NameIndex", IsUnique = true)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(MaxStringLength)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
