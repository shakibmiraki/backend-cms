using Application.Services.Models;
using Domain.Core.Entities.Identities;

namespace Service.Rest.Models.Roles
{
    public class GetClientRoleByIdResponse : BaseResponseModel
    {
        public ClientRole Role { get; set; }
    }
}
