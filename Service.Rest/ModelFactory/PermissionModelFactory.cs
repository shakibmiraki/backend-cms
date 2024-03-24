using Domain.Core.Entities.Identities;
using Service.Rest.Models.Permissions;

namespace IdentityMicroservice.ModelFactory
{
    public class PermissionModelFactory : IPermissionModelFactory
    {
        public IEnumerable<PermissionModel> PreparePermissionModel(IEnumerable<PermissionRecord> permissions)
        {
            var permissionsViewModel = new List<PermissionModel>();

            foreach (var permission in permissions)
            {
                permissionsViewModel.Add(new PermissionModel
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    SystemName = permission.SystemName
                });
            }

            return permissionsViewModel;
        }
    }
}