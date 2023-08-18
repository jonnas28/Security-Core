using Identity.Common;
using Identity.Model;
using Identity.Repository.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Response;

namespace WebAPI.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private IAuthenticateBL _authenticate;
        public AuthenticateController(UserManager<User> userManager, IConfiguration configuration, IAuthenticateBL authenticate)
        {
            _userManager = userManager;
            _configuration = configuration;
            _authenticate= authenticate;
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(
            Summary = "Register",
            Description = "Adding user account to Identity server"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RegisterDTO>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return StatusCode(403,new ApiResponse(403, "User already exists!"));
            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(500, JsonConvert.SerializeObject(result.Errors)));
            return Ok(new ApiOkResponse(model));
        }

        [HttpPost]
        [Route("login")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokenDTO))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await _authenticate.Login(user);
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                user.RefreshToken = _authenticate.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                await _userManager.UpdateAsync(user);

                var ret = new TokenDTO
                {
                    expiration = token.ValidTo,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiration = user.RefreshTokenExpiryTime,
                };

                return Ok(ret);
            }
            return Unauthorized(new ApiResponse(401));
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Assign-role")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> AssignRole([FromQuery] string username, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest(new ApiResponse(404, "User does not exists!"));

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest(new ApiResponse(500, "Role does not exist"));
            await _userManager.AddToRoleAsync(user, roleName);

            return Ok();
        }
    }    
}
