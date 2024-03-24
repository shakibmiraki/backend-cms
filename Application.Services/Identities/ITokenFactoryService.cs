using Domain.Core.Entities.Identities;

namespace Application.Services.Identities
{
    public interface ITokenFactoryService
    {
        Task<JwtTokensData> CreateJwtTokens(Client client);

        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
