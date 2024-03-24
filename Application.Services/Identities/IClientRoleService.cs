using Domain.Core.Entities.Identities;

namespace Application.Services.Identities
{
    public interface IClientRoleService
    {

        void DeleteClientRole(ClientRole clientRole);

        Task<ClientRole?> GetClientRoleById(long clientRoleId);

        Task<ClientRole?> GetClientRole(Client client);

        Task<IEnumerable<ClientRole>> GetAllClientRoles();

        Task<ClientRole?> GetClientRoleBySystemName(string systemName);
    }
}