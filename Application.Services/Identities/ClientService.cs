using Domain.Core;
using Domain.Core.Entities.Identities;
using Infrastructure.Data.EntityFramework.PageList;

namespace Application.Services.Identities
{
    public partial class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IPagedList<Client>> GetAllClients(int pageIndex, int pageSize, bool isDelete = false)
        {
            var users = _unitOfWork.Clients.GetAllPaged(a => a.Deleted == isDelete);
            return await users.ToPagedListAsync(pageIndex, pageSize);
        }


        public async Task DeleteClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (client.IsSystemUser)
                throw new InvalidOperationException("system users should not be deleted");

            client.Deleted = true;
            _unitOfWork.Clients.Update(client);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Client?> GetClientById(long clientId)
        {
            return await _unitOfWork.Clients.GetAsync(clientId);
        }

        public async Task<IList<Client>> GetClientsByIds(long[] clientIds)
        {
            if (clientIds == null || clientIds.Length == 0)
                return new List<Client>();

            var query = await _unitOfWork.Clients.GetAll(u => clientIds.Contains(u.Id) && !u.Deleted);
            return query.ToList();
        }

        public async Task<Client?> GetClientByClientname(string clientname)
        {
            if (string.IsNullOrWhiteSpace(clientname))
                return null;

            var user = await _unitOfWork.Clients.GetAll(u => u.Clientname == clientname);
            return user.SingleOrDefault();
        }

    }
}