using Domain.Core.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFramework
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
         : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);                        
        }

        public virtual DbSet<ClientRole> ClientRoles { get; set; }
        public virtual DbSet<PermissionRecord> PermissionRecords { get; set; }
        public virtual DbSet<Client> Clients { get; set; }



    }
}
