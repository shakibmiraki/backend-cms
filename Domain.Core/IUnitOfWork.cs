using Domain.Core.Entities.Users;

namespace Domain.Core
{
    public interface IUnitOfWork
    {
        IClientRepository Clients { get; }

        IPermissionRecordRepository PermissionRecords { get; }

        IClientRoleRepository ClientRoles { get; }

        Task CommitAsync();
    }
}
