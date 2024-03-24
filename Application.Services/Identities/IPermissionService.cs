using Domain.Core.Entities.Identities;

namespace Application.Services.Identities
{
    public interface IPermissionService
    {
        Task DeletePermissionRecordAsync(PermissionRecord permission);

        Task<IEnumerable<PermissionRecord>> GetPermissionRecords();

        Task<PermissionRecord> GetPermissionRecordById(long id);

        Task<IEnumerable<PermissionRecord>> GetPermissionRecordsByRoleId(long roleId);

    }
}
