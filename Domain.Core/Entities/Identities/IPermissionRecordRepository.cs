using Domain.Core.Entities.Identities;
using System.Linq.Expressions;

namespace Domain.Core.Entities.Users
{
    public interface IPermissionRecordRepository
    {
        Task<IEnumerable<PermissionRecord>> GetAll();

        Task<IEnumerable<PermissionRecord>> GetAll(Expression<Func<PermissionRecord, bool>> where);

        IQueryable<PermissionRecord> GetAllPaged();

        IQueryable<PermissionRecord> GetAllPaged(Expression<Func<PermissionRecord, bool>> @where);

        Task<PermissionRecord?> GetAsync(long id);

        Task<PermissionRecord> AddAsync(PermissionRecord permissionRecord);

        void Update(PermissionRecord permissionRecord);

        void Delete(PermissionRecord permissionRecord);
    }
}
