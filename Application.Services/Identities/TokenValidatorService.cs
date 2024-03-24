using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Application.Services.Identities
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IClientService _clientService;

        public TokenValidatorService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var customerIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!long.TryParse(customerIdString, out long customerId))
            {
                context.Fail("This is not our issued token. It has no customer-id.");
                return;
            }

            var customer = await _clientService.GetClientById(customerId);
            if (customer == null || !customer.Active)
            {
                // customer has changed his/her password/roles/stat/IsActive
                context.Fail("This token is expired. Please login again.");
            }
        }
    }
}
