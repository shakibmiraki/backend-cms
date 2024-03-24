using Domain.Core.Entities.Identities;
using Domain.Core;

namespace Application.Services.Identities
{
    public class ClientRoleService : IClientRoleService
    {
        private IUnitOfWork _unitOfWork;

        public ClientRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteClientRole(ClientRole clientRole)
        {
            if (clientRole == null)
                throw new ArgumentNullException(nameof(clientRole));

            if (clientRole.IsSystemRole)
                throw new InvalidOperationException("system role should not be deleted");

            _unitOfWork.ClientRoles.Delete(clientRole);
            _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ClientRole>> GetAllClientRoles()
        {
            var roles = _unitOfWork.ClientRoles.GetAll();

            return await roles;
        }

        public async Task<ClientRole?> GetClientRoleById(long clientRoleId)
        {
            if (clientRoleId <= 0)
                return null;

            var roles = await _unitOfWork.ClientRoles.GetAll(ur => ur.Id == clientRoleId && ur.Active);
            return roles.FirstOrDefault();
        }

        public async Task<ClientRole?> GetClientRole(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var result = await _unitOfWork.Clients.GetAsync(client.Id);
            return result?.ClientRole;
        }

        public async Task<ClientRole?> GetClientRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var clientRoles = await _unitOfWork.ClientRoles.GetAll(a => a.SystemName == systemName);
            return clientRoles.SingleOrDefault();
        }


    }
}