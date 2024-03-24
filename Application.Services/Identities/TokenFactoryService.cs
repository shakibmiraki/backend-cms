using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Identities;
using Domain.Core.Entities.Identities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Identities
{

    public class JwtTokensData
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string RefreshTokenSerial { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }

    public class JwtConfig
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenExpirationMinutes { get; set; }

        public int RefreshTokenExpirationMinutes { get; set; }

    }


    public class TokenFactoryService : ITokenFactoryService
    {
        private readonly IClientService _clientService;
        private readonly IEncryptionService _encryptionService;
        private readonly IClientRoleService _roleService;
        private readonly JwtConfig _jwtConfig;
        private readonly ILogger<TokenFactoryService> _logger;

        public TokenFactoryService(
            IClientService clientService,
            IEncryptionService encryptionService,
            IClientRoleService roleService,
            JwtConfig jwtConfig,
            ILogger<TokenFactoryService> logger)
        {
            _clientService = clientService;
            _encryptionService = encryptionService;
            _roleService = roleService;
            _jwtConfig = jwtConfig;
            _logger = logger;
        }


        public async Task<JwtTokensData> CreateJwtTokens(Client client)
        {
            var (accessToken, claims) = await CreateAccessToken(client);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) CreateRefreshToken()
        {
            var refreshTokenSerial = _encryptionService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, _encryptionService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer, ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _jwtConfig.Issuer),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _jwtConfig.Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtConfig.RefreshTokenExpirationMinutes),
                signingCredentials: creds);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                    out _
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(TokenFactoryService)}-->{nameof(GetRefreshTokenSerial)}",ex);
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

        private async Task<(string AccessToken, IEnumerable<Claim> Claims)> CreateAccessToken(Client client)
        {
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti,
                    _encryptionService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String,
                    _jwtConfig.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer, ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64, _jwtConfig.Issuer),

                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),

                
                // custom data
                new Claim(ClaimTypes.UserData, client.Id.ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),

                new Claim(ClaimTypes.Name, "Cms", ClaimValueTypes.String,
                    _jwtConfig.Issuer)
            };

            // add permissions to token
            var userRoles = await _roleService.GetClientRole(client);
            var permissions = userRoles.PermissionRecords;
            claims.AddRange(permissions.Select(permission => new Claim("permissions", permission.SystemName, ClaimValueTypes.String, _jwtConfig.Issuer)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes),
                signingCredentials: credentials);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}
