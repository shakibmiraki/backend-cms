using Domain.Core.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityMicroservice.Data.Configuration
{
    public class PermissionRecordConfiguration : IEntityTypeConfiguration<PermissionRecord>
    {
        public void Configure(EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.ToTable("PermissionRecords");
            builder.HasKey(e => e.Id);
            builder.HasIndex(a => a.SystemName).IsUnique();
        }
    }
}
