using Identity.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Repository.Contracts
{
    public interface IAuthenticateBL
    {
        Task<JwtSecurityToken> Login(User user);
        JwtSecurityToken GetToken(List<Claim> authClaims);
        string GenerateRefreshToken();
    }
}
