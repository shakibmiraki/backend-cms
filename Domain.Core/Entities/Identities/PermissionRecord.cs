using Domain.Core;

namespace Domain.Core.Entities.Identities
{
    public partial class PermissionRecord : BaseEntity<long>
    {

        public string Name { get; set; }

        public string SystemName { get; set; }

        public List<ClientRole> ClientRoles { get; set; } = new List<ClientRole>();

    }
}