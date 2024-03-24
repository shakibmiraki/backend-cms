using Domain.Core.Entities.Identities;
using System.Linq.Expressions;

namespace Domain.Core.Entities.Users
{
    public interface IClientRoleRepository
    {
        Task<IEnumerable<ClientRole>> GetAll();

        Task<IEnumerable<ClientRole>> GetAll(Expression<Func<ClientRole , bool>> where);

        IQueryable<ClientRole> GetAllPaged();

        IQueryable<ClientRole> GetAllPaged(Expression<Func<ClientRole, bool>> @where);

        Task<ClientRole?> GetAsync(long id);

        Task<ClientRole> AddAsync(ClientRole clientRole);

        void Update(ClientRole clientRole);

        void Delete(ClientRole clientRole);
    }
}
