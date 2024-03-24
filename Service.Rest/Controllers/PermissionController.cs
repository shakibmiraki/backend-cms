using Microsoft.AspNetCore.Mvc;
using IdentityMicroservice.ModelFactory;
using Application.Services.Models;
using Application.Services.Identities;
using Service.Rest.Models.Permissions;
using Middleware;


namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IPermissionModelFactory _permissionModelFactory;

        public PermissionController(ILogger<PermissionController> logger, IPermissionService permissionService, IPermissionModelFactory permissionModelFactory)
        {
            _logger = logger;
            _permissionService = permissionService;
            _permissionModelFactory = permissionModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new GetPermissionsResponse { Result = ResultType.Error };
            try
            {
                var permissions = await _permissionService.GetPermissionRecords();
                response.Permissions = _permissionModelFactory.PreparePermissionModel(permissions);
                response.Result = ResultType.Success;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Messages.Add(ConstMessage.ErrorMessage);
                return BadRequest(response);
            }
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(long roleId)
        {
            var response = new GetPermissionsByRoleIdResponse { Result = ResultType.Error };
            try
            {
                response.Permissions = await _permissionService.GetPermissionRecordsByRoleId(roleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Messages.Add(ConstMessage.ErrorMessage);
                return BadRequest(response);
            }
        }
    }
}
