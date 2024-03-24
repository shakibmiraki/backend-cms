using Domain.Core.Entities.Identities;
using Service.Rest.Models.Permissions;

namespace IdentityMicroservice.ModelFactory
{
    public interface IPermissionModelFactory
    {
        IEnumerable<PermissionModel> PreparePermissionModel(IEnumerable<PermissionRecord> permissions);
    }
}