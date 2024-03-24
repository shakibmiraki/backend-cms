using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Application.Services.Identities
{
    public interface ITokenValidatorService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}
