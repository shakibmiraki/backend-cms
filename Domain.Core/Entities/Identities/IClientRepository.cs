using Domain.Core.Entities.Identities;
using System.Linq.Expressions;

namespace Domain.Core.Entities.Users
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll();

        Task<IEnumerable<Client>> GetAll(Expression<Func<Client, bool>> where);
        
        IQueryable<Client> GetAllPaged();

        IQueryable<Client> GetAllPaged(Expression<Func<Client, bool>> @where);

        Task<Client?> GetAsync(long id);

        Task<Client> AddAsync(Client client);

        void Update(Client client);
    }
}
