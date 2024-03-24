using Domain.Core.Entities.Identities;
using Domain.Core;

namespace Application.Services.Identities
{
    public class PermissionService : IPermissionService
    {

        private readonly IClientRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService( IClientRoleService roleService, IUnitOfWork unitOfWork)
        {
            _roleService = roleService;
            _unitOfWork = unitOfWork;
        }


        public async Task DeletePermissionRecordAsync(PermissionRecord permission)
        {
            _unitOfWork.PermissionRecords.Delete(permission);
            await _unitOfWork.CommitAsync();
        }


        public async Task<IEnumerable<PermissionRecord>> GetPermissionRecords()
        {
            var permissions = await _unitOfWork.PermissionRecords.GetAll();
            return permissions.ToList();
        }

        public async Task<PermissionRecord?> GetPermissionRecordById(long id)
        {
            return await _unitOfWork.PermissionRecords.GetAsync(id);
        }

        public async Task<IEnumerable<PermissionRecord>> GetPermissionRecordsByRoleId(long roleId)
        {
            var permissions = await _roleService.GetClientRoleById(roleId);
            return permissions.PermissionRecords.ToList();
        }

    }
}
