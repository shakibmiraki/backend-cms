using Domain.Core;

namespace Domain.Core.Entities.Identities
{
    public partial class ClientRole : BaseEntity<long>
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public bool IsSystemRole { get; set; }

        public bool Active { get; set; }

        public List<PermissionRecord> PermissionRecords { get; set; } = new List<PermissionRecord>();
    }
}