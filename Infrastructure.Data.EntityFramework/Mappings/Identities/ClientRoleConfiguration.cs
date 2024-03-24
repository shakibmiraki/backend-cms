using Domain.Core.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityMicroservice.Data.Configuration
{
    public class ClientRoleConfiguration : IEntityTypeConfiguration<ClientRole>
    {
        public void Configure(EntityTypeBuilder<ClientRole> builder)
        {
            builder.ToTable("ClientRoles");
            builder.HasKey(e => e.Id);
            builder.HasIndex(a => a.SystemName).IsUnique();
        }
    }
}
