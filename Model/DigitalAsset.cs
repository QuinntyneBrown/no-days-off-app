using System;

namespace NoDaysOffApp.Model
{
    public class DigitalAsset: ILoggable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? FileModified { get; set; }
        public long? Size { get; set; }
        public string ContentType { get; set; }
        public string RelativePath { get { return $"api/digitalasset/serve?uniqueid={UniqueId}"; } }
        public Byte[] Bytes { get; set; } = new byte[0];
        public Guid? UniqueId { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
