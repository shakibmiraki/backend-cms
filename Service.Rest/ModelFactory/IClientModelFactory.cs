using System.Threading.Tasks;
using Domain.Core.Entities.Identities;
using Service.Rest.Models.Clients;

namespace IdentityMicroservice.ModelFactory
{
    public interface IClientModelFactory
    {
        Task<Client> PreparePostClientRequestToClient(PostClientRequest request);
    }
}
