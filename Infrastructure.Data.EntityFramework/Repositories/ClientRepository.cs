using Domain.Core.Entities.Identities;
using Domain.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.EntityFramework.Repositories
{
    public class ClientRepository : IClientRepository
    {

        private readonly DbSet<Client> DbSet;

        public ClientRepository(DatabaseContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            DbSet = context.Set<Client>();
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetAll(Expression<Func<Client, bool>> where)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        public IQueryable<Client> GetAllPaged()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Client> GetAllPaged(Expression<Func<Client, bool>> @where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        public Task<Client?> GetAsync(long id)
        {
            var query = DbSet.Include(a=>a.ClientRole).Include(a=>a.ClientRole.PermissionRecords).SingleOrDefaultAsync(a=>a.Id == id);
            return query;
        }

        public async Task<Client> AddAsync(Client client)
        {
            ArgumentNullException.ThrowIfNull(client);
            var entity = await DbSet.AddAsync(client);
            return entity.Entity;
        }

        public void Update(Client client)
        {
            ArgumentNullException.ThrowIfNull(client);
            DbSet.Update(client);
        }
    }
}


