using Domain.Core.Entities.Identities;
using Domain.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.EntityFramework.Repositories
{
    public class ClientRoleRepository : IClientRoleRepository
    {

        private readonly DbSet<ClientRole> DbSet;

        public ClientRoleRepository(DatabaseContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            DbSet = context.Set<ClientRole>();
        }

        public async Task<IEnumerable<ClientRole>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<ClientRole>> GetAll(Expression<Func<ClientRole, bool>> where)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        public IQueryable<ClientRole> GetAllPaged()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<ClientRole> GetAllPaged(Expression<Func<ClientRole, bool>> @where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        public Task<ClientRole?> GetAsync(long id)
        {
            var query = DbSet.SingleOrDefaultAsync(a => a.Id == id);
            return query;
        }

        public async Task<ClientRole> AddAsync(ClientRole clientRole)
        {
            ArgumentNullException.ThrowIfNull(clientRole);
            var entity = await DbSet.AddAsync(clientRole);
            return entity.Entity;
        }

        public void Update(ClientRole clientRole)
        {
            ArgumentNullException.ThrowIfNull(clientRole);
            DbSet.Update(clientRole);
        }

        public void Delete(ClientRole entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            DbSet.Remove(entity);
        }
    }
}


