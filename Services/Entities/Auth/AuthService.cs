using Microsoft.IdentityModel.Tokens;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Services.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Persistence.Configuration.Interfaces.Auth;
using System.Security.Cryptography;

namespace SistemasSalgados.Services.Entities.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ITokenConfiguration _tokenConfiguration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, ITokenConfiguration tokenConfiguration, IRefreshTokenService refreshTokenService, ILogger<AuthService> logger)
        {
            _userService = userService;
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenConfiguration = tokenConfiguration;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
        }

        private List<Claim> GenerateClaimsByPerson(User user)
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };

            return userClaims;
        }

        private async Task<AuthenticatedUser> Authenticate(int userId)
        {
            User user = await _userService.SelectById(userId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            List<Claim> userClaims = GenerateClaimsByPerson(user);
            WebToken webToken = GenerateToken(userClaims);
            RefreshToken refreshToken = await GenerateRefreshToken(user.Id);

            if (refreshToken == null || webToken == null)
                throw new Exception("Erro ao criar token");

            return new AuthenticatedUser
            {
                User = user,
                RefreshToken = refreshToken,
                WebToken = webToken,
            };
        }

        public async Task<AuthenticatedUser> AuthenticateWithRefreshToken(Guid token)
        {
            RefreshToken refreshToken = await _refreshTokenService.Select().FirstOrDefaultAsync(p => p.Token == token);

            if (refreshToken == null || DateTime.Now > refreshToken.ExpiresIn)
                return null;

            User user = await _userService.Select()
                                            .FirstOrDefaultAsync(p => p.Id == refreshToken.UserId.Value);

            if (user == null)
                return null;

            return await Authenticate(user.Id);
        }

        public async Task<AuthenticatedUser> AuthenticateWithPassword(string password, User user)
        {
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return await Authenticate(user.Id);
        }

        private async Task<RefreshToken> GenerateRefreshToken(int userId)
        {
            RefreshToken refreshToken = new()
            {
                UserId = userId,
                Token = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                ExpiresIn = DateTime.UtcNow.AddSeconds(_tokenConfiguration.RefreshTokenExpiresIn),
            };

            var createdResult = await _refreshTokenService.Create(refreshToken);
            await _refreshTokenService.DeleteExpiredTokens();

            return !createdResult.HasError ? refreshToken : null;
        }

        public WebToken GenerateToken(List<Claim> claims, int? expiresIn = null)
        {
            List<Claim> claimList = claims;
            claimList.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(Guid.NewGuid().ToString(), "TokenIdentifier"), claimList);

            DateTime expirationDate = DateTime.UtcNow.AddSeconds(expiresIn ?? _tokenConfiguration.DefaultExpiresIn);
            var key = Encoding.ASCII.GetBytes(_tokenConfiguration.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _tokenConfiguration.Audience,
                Issuer = _tokenConfiguration.Issuer,
                Expires = expirationDate,
                NotBefore = DateTime.Now,
                Subject = identity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);

            return new WebToken
            {
                Token = _tokenHandler.WriteToken(securityToken),
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = expirationDate,
            };
        }

        public Task<bool> VerifyToken(string token)
        {
            byte[] key = Encoding.UTF8.GetBytes(_tokenConfiguration.SecretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _tokenConfiguration.Issuer,
                ValidAudience = _tokenConfiguration.Audience,
                IssuerSigningKey = securityKey,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true
            };

            if (!_tokenHandler.CanReadToken(token))
                return Task.FromResult(false);

            try
            {
                var claimsPrincipal = _tokenHandler.ValidateToken(token, validationParameters, out _);
                return Task.FromResult(claimsPrincipal.Claims.Any());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.FromResult(false);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
