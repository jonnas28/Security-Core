using AutoMapper;
using Helper.Common;
using Identity.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Payroll.Common;
using Payroll.Core.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection.Metadata;
using WebAPI.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        IMapper _mapper;
        private RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        // GET: api/<RoleController>
        [HttpGet]
        [SwaggerOperation(
            Summary = "List Role",
            Description = "Returns list of roles from Identity server",
            OperationId = "RolesGET"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<List<IdentityRoleDTO>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Get([FromQuery] PageConfig pageConfig)
        {
            var query = _roleManager.Roles;
            //return Ok(roles);
            var result = await PagedList<IdentityRole>.ToPagedListAsync(query, pageConfig.PageNumber, pageConfig.PageSize);

            var metadata = new PageMetadata
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                HasNext = result.HasNext,
                HasPrevious = result.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(new ApiOkResponse(_mapper.Map<List<IdentityRoleDTO>>(result), metadata));
        }

        // POST api/<RoleController>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add Role",
            Description = "Adding roles to Identity server",
            OperationId = "RolesADD"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IdentityRoleDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Post(IdentityRoleDTO value)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(value.NormalizedName);
                if (roleExist)
                {
                    return BadRequest(new ApiResponse(400, "Role already exists!"));
                }
                await _roleManager.CreateAsync(_mapper.Map<IdentityRole>(value));
                return Ok(new ApiOkResponse(value));
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete Role",
            Description = "Deleting roles from Identity server",
            OperationId = "RolesDELETE"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IdentityRoleDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var roleExist = await _roleManager.FindByIdAsync(id);
                if(roleExist == null) {
                    return BadRequest(new ApiResponse(400, "Invalid, Role id doesnt exists!"));
                }
                await _roleManager.DeleteAsync(roleExist);
                return Ok(new ApiOkResponse(_mapper.Map<IdentityRoleDTO>(roleExist)));
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
    }
}
