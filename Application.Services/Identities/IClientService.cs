using Domain.Core.Entities.Identities;
using Infrastructure.Data.EntityFramework.PageList;

namespace Application.Services.Identities
{

    public partial interface IClientService
    {

        Task<IPagedList<Client>> GetAllClients(int pageIndex, int pageSize, bool isDelete = false);

        Task DeleteClient(Client client);

        Task<Client?> GetClientById(long id);

        Task<IList<Client>> GetClientsByIds(long[] clientIds);

        Task<Client?> GetClientByClientname(string clientname);
    }
}