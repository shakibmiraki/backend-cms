using Application.Services.Models;
using Domain.Core.Entities.Identities;

namespace Service.Rest.Models.Permissions
{
    public class GetPermissionsByRoleIdResponse : BaseResponseModel
    {
        public IEnumerable<PermissionRecord> Permissions { get; set; }
    }
}
