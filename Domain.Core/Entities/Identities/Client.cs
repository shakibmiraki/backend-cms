using Domain.Core;
using System;

namespace Domain.Core.Entities.Identities
{
    public class Client : BaseEntity<long>, IAuditEntity
    {
        public string Clientname { get; set; }

        public string Password { get; set; }

        public bool Deleted { get; set; }

        public bool Active { get; set; }

        public bool IsSystemUser { get; set; }

        public ClientRole ClientRole { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
