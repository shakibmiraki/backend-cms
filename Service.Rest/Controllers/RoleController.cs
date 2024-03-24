using Microsoft.AspNetCore.Mvc;
using IdentityMicroservice.ModelFactory;
using Microsoft.AspNetCore.Http.Extensions;
using Application.Services.Models;
using Application.Services.Identities;
using Domain.Core.Entities.Users;
using Service.Rest.Models.Roles;
using Domain.Core;
using Middleware;


namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IClientRoleService _clientRoleService;
        private readonly IClientRoleModelFactory _clientRoleModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IUnitOfWork _unitOfWork;


        public RoleController(
            ILogger<RoleController> logger,
            IClientRoleService clientRoleService,
            IClientRoleModelFactory clientRoleModelFactory, IPermissionService permissionService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _clientRoleService = clientRoleService;
            _clientRoleModelFactory = clientRoleModelFactory;
            _permissionService = permissionService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetClientRoles")]
        public async Task<IActionResult> Get()
        {
            var response = new GetClientRoleResponse { Result = ResultType.Error };
            try
            {

                var roles = await _clientRoleService.GetAllClientRoles();

                response.Roles = roles;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var response = new GetClientRoleByIdResponse { Result = ResultType.Error };
            try
            {
                var role = await _clientRoleService.GetClientRoleById(id);

                if (role is null)
                {
                    response.Messages.Add("role not found");
                    return NotFound(response);
                }

                response.Role = role;
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

        [HttpPost("CreateClientRole")]
        public async Task<IActionResult> Post([FromBody] PostClientRoleRequest request)
        {
            var response = new PostClientRoleResponse { Result = ResultType.Error };

            try
            {

                if (!ModelState.IsValid)
                {
                    response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(response);
                }

                var clientRole = await _clientRoleModelFactory.PreparePostClientRoleRequestToClientRole(request);

                var isExist = await _clientRoleService.GetClientRoleBySystemName(clientRole.SystemName);
                if (isExist is not null)
                {
                    response.Messages.Add("role has exist");
                    return BadRequest(response);
                }

                await _unitOfWork.ClientRoles.AddAsync(clientRole);
                await _unitOfWork.CommitAsync();
                response.Result = ResultType.Success;
                response.Messages.Add(ConstMessage.SuccessMessage);
                return Created(new Uri(Request.GetDisplayUrl() + "/" + clientRole.Id), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Messages.Add(ConstMessage.ErrorMessage);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PutClientRoleRequest request)
        {
            var response = new PutClientRoleResponse { Result = ResultType.Error };
            try
            {
                if (!ModelState.IsValid)
                {
                    response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(response);
                }

                var role = await _clientRoleService.GetClientRoleById(id);
                foreach (var permission in role.PermissionRecords)
                {
                    role.PermissionRecords.Remove(permission);
                }

                foreach (var permissionId in request.PermissionIds)
                {
                    var permission = await _permissionService.GetPermissionRecordById(permissionId);
                    role.PermissionRecords.Add(permission);
                }

                role.SystemName = request.SystemName;
                role.Name = request.Name;
                role.Active = request.Active;

                _unitOfWork.ClientRoles.Update(role);
                await _unitOfWork.CommitAsync();

                response.Messages.Add(ConstMessage.SuccessMessage);
                response.Result = ResultType.Success;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Messages.Add(ConstMessage.ErrorMessage);
                return Ok(response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new DeleteClientRoleResponse { Result = ResultType.Error };
            try
            {
                var clientRole = await _clientRoleService.GetClientRoleById(id);
                if (clientRole is null)
                {
                    response.Messages.Add("role not found");
                    return NotFound(response);
                }

                _clientRoleService.DeleteClientRole(clientRole);
                response.Messages.Add(ConstMessage.SuccessMessage);
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
    }
}
