using Microsoft.AspNetCore.Mvc;
using IdentityMicroservice.ModelFactory;
using Microsoft.AspNetCore.Http.Extensions;
using Application.Services.Models;
using Application.Services.Identities;
using Service.Rest.Models.Clients;
using Domain.Core;
using Middleware;


namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientModelFactory _clientModelFactory;
        private readonly IClientRegistrationService _clientRegistrationService;
        private readonly IClientService _clientService;
        private readonly IClientRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;

        public ClientController(
            ILogger<ClientController> logger,
            IClientModelFactory userModelFactory,
            IClientRegistrationService userRegistrationService,
            IClientService userService,
            IClientRoleService roleService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _clientModelFactory = userModelFactory;
            _clientRegistrationService = userRegistrationService;
            _clientService = userService;
            _roleService = roleService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetClients")]
        public async Task<IActionResult> Post([FromBody] GetClientRequest request)
        {
            var response = new GetClientResponse { Result = ResultType.Error };
            try
            {
                var clients = await _clientService.GetAllClients(request.PageIndex, request.PageSize);

                response.Clients = clients;
                response.TotalCount = clients.TotalCount;
                response.Page = clients.PageIndex;
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
            var response = new GetClientByIdResponse { Result = ResultType.Error };
            try
            {
                var client = await _clientService.GetClientById(id);

                if (client is null)
                {
                    response.Messages.Add("کاربری با این مشخصات یافت نشد");
                    return NotFound(response);
                }

                response.Client = client;
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

        [HttpPost("CreateClient")]
        public async Task<IActionResult> Post([FromBody] PostClientRequest request)
        {
            var response = new PostClientResponse { Result = ResultType.Error };
            try
            {
                var client = await _clientModelFactory.PreparePostClientRequestToClient(request);

                var result = await _clientRegistrationService.ValidateClient(client);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    response.Messages.Add(result);
                    return BadRequest(response);
                }

                await _unitOfWork.Clients.AddAsync(client);
                await _unitOfWork.CommitAsync();
                response.Result = ResultType.Success;
                response.Messages.Add(ConstMessage.SuccessMessage);
                return Created(new Uri(Request.GetDisplayUrl() + "/" + client.Id), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Messages.Add(ConstMessage.ErrorMessage);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PutClientRequest request)
        {
            var response = new PutClientResponse { Result = ResultType.Error };
            try
            {

                if (!ModelState.IsValid)
                {
                    response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(response);
                }

                var user = await _clientService.GetClientById(id);

                var userRole = await _roleService.GetClientRoleById(request.ClientRoleId);
                user.ClientRole = userRole;
                user.Clientname = request.Clientname;
                user.Active = request.Active;

                _unitOfWork.Clients.Update(user);
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
            var response = new DeleteClientResponse { Result = ResultType.Error };
            try
            {
                var client = await _clientService.GetClientById(id);
                if (client is null)
                {
                    response.Messages.Add("client not found");
                    return NotFound(response);
                }

                await _clientService.DeleteClient(client);
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
