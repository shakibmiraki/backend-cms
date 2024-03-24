using Domain.Core;
using Domain.Core.Entities.Users;
using Infrastructure.Data.EntityFramework.Repositories;

namespace Infrastructure.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;

        public IClientRepository Clients { get; private set; }

        public IClientRoleRepository ClientRoles { get; private set; }

        public IPermissionRecordRepository PermissionRecords { get; private set; }


        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

            Clients = new ClientRepository(_context);
            ClientRoles = new ClientRoleRepository(_context);
            PermissionRecords = new PermissionRecordRepository(_context);
        }


        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
