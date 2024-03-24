using Domain.Core;
using Domain.Core.Entities.Identities;
using Service.Rest.Models.Roles;

namespace IdentityMicroservice.ModelFactory
{
    public class ClientRoleModelFactory : IClientRoleModelFactory
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientRoleModelFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientRole> PreparePostClientRoleRequestToClientRole(PostClientRoleRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var model = new ClientRole
            {
                Active = request.Active,
                IsSystemRole = false,
                Name = request.Name,
                SystemName = request.SystemName,
                PermissionRecords = new List<PermissionRecord>()
            };

            foreach (var permissionId in request.PermissionIds)
            {
                var permission = await _unitOfWork.PermissionRecords.GetAsync(permissionId);
                model.PermissionRecords.Add(permission);
            }

            return model;
        }
    }
}