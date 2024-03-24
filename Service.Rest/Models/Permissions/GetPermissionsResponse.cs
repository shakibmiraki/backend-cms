using Application.Services.Models;

namespace Service.Rest.Models.Permissions
{
    public class GetPermissionsResponse : BaseResponseModel
    {
        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}
