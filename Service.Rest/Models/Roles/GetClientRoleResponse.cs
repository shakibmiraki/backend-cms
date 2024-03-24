using Application.Services.Models;
using Domain.Core.Entities.Identities;

namespace Service.Rest.Models.Roles
{
    public class GetClientRoleResponse : BaseResponseModel
    {
        public IEnumerable<ClientRole> Roles { get; set; }

        public int TotalCount { get; set; }

        public int Page { get; set; }
    }
}
