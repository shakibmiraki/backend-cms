using Domain.Core.Entities.Identities;
using Domain.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.EntityFramework.Repositories
{
    public class PermissionRecordRepository : IPermissionRecordRepository
    {

        private readonly DbSet<PermissionRecord> DbSet;

        public PermissionRecordRepository(DatabaseContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            DbSet = context.Set<PermissionRecord>();
        }

        public async Task<IEnumerable<PermissionRecord>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<PermissionRecord>> GetAll(Expression<Func<PermissionRecord, bool>> where)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        public IQueryable<PermissionRecord> GetAllPaged()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<PermissionRecord> GetAllPaged(Expression<Func<PermissionRecord, bool>> @where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        public Task<PermissionRecord?> GetAsync(long id)
        {
            var query = DbSet.SingleOrDefaultAsync(a => a.Id == id);
            return query;
        }

        public async Task<PermissionRecord> AddAsync(PermissionRecord permissionRecord)
        {
            ArgumentNullException.ThrowIfNull(permissionRecord);
            var entity = await DbSet.AddAsync(permissionRecord);
            return entity.Entity;
        }

        public void Update(PermissionRecord permissionRecord)
        {
            ArgumentNullException.ThrowIfNull(permissionRecord);
            DbSet.Update(permissionRecord);
        }

        public void Delete(PermissionRecord entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            DbSet.Remove(entity);
        }
    }
}


