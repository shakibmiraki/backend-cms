using Microsoft.AspNetCore.Mvc;
using Application.Services.Identities;
using Application.Services.Models;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IClientRegistrationService _clientRegistrationService;
        private readonly IClientService _clientService;
        private readonly ITokenFactoryService _tokenFactoryService;


        public IdentityController(IClientRegistrationService clientRegistrationService,
            IClientService clientService,
            ITokenFactoryService tokenFactoryService)
        {
            _clientRegistrationService = clientRegistrationService;
            _clientService = clientService;
            _tokenFactoryService = tokenFactoryService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = new LoginResponse { Result = ResultType.Error };

            if (!ModelState.IsValid)
            {
                response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(response);
            }

            var loginResult = await _clientRegistrationService.LoginClient(model.Username, model.Password);

            switch (loginResult)
            {
                case ClientLoginResults.Successful:
                    {
                        var customer = await _clientService.GetClientByClientname(model.Username);

                        var token = await _tokenFactoryService.CreateJwtTokens(customer);

                        response.Result = ResultType.Success;
                        response.AccessToken = token.AccessToken;
                        response.RefreshToken = token.RefreshToken;

                        return Ok(response);
                    }
                case ClientLoginResults.CustomerNotExist:
                    response.Messages.Add(
                        "client not found");
                    break;
                case ClientLoginResults.Deleted:
                    response.Messages.Add("client has been deleted");
                    break;
                case ClientLoginResults.NotActive:
                    response.Messages.Add(
                        "client is not acitve");
                    break;
                case ClientLoginResults.NotRegistered:
                    response.Messages.Add(
                        "client has not registered");
                    break;
                case ClientLoginResults.LockedOut:
                    response.Messages.Add(
                        "client has been locked");
                    break;
                case ClientLoginResults.WrongPassword:
                    response.Messages.Add(
                       "username or password is wrong");
                    break;
                default:
                    response.Messages.Add(
                        "wrong credentials");
                    break;
            }
            return BadRequest(response);

        }

    }
}
