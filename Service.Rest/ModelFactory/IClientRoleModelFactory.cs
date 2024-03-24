using Domain.Core.Entities.Identities;
using Service.Rest.Models.Roles;

namespace IdentityMicroservice.ModelFactory
{
    public interface IClientRoleModelFactory
    {
        Task<ClientRole> PreparePostClientRoleRequestToClientRole(PostClientRoleRequest request);
    }
}
