using AutoMapper;
using FluentValidation.Results;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payroll.Common;
using Payroll.Common.Paramaters;
using Payroll.Core.Model;
using Payroll.Core.Repository;
using Payroll.Core.Validator;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using WebAPI.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers.Payroll.Masterfile
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IRepositoryWrapper _repository;
        IMapper _mapper;
        //ILoggerManager _logger;
        public EmployeeController(
            IRepositoryWrapper repositoryWrapper, 
            IMapper mapper
            //ILoggerManager logger
           )
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
            //_logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<List<EmployeeDTO>>))]
        [SwaggerOperation(
            Summary = "Get Employee",
            Description = "Returns/retrieves a collection of Employee",
            OperationId = "GetEmployee"
        )]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromQuery] EmployeeParamater parameter)
        {
            try
            {
                var result = await _repository.Employee.GetAllAsync(parameter);
                if (result == null)
                    return NotFound();
                else
                {
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
                    return Ok(new ApiOkResponse(_mapper.Map<List<EmployeeDTO>>(result), metadata));
                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add Employee",
            Description = "Create and saves employee account information data to the database",
            OperationId = "AddEmployee"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<EmployeeDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] EmployeeDTO value)
        {
            try
            {
                //value.OrgId = Guid.Parse(orgID);

                ValidationResult response = new ValidationResult();
                response = new EmployeeValidator().Validate(value);
                if (!response.IsValid)
                {
                    response.Errors.ForEach(x =>
                    {
                        ModelState.AddModelError(x.PropertyName, x.ErrorMessage);
                    });
                    return BadRequest(new ApiBadRequestResponse(ModelState));
                }
                _repository.Employee.Create(_mapper.Map<Employee>(value));

                await _repository.SaveAsync();
                //_logger.LogInfo($"Insert Employee {JsonConvert.SerializeObject(value)} Successful.");
                return Ok(new ApiOkResponse<EmployeeDTO>(value));
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                return BadRequest(new ApiResponse(500));
            }
        }

        [HttpGet("{Id}")]
        [SwaggerOperation(
            Summary = "Get Employee by id",
            Description = "Retrieves an Employee's information base on Employee id parameter",
            OperationId = "GetEmployee"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<EmployeeDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByID(int Id)
        {
            try
            {
                var result = await _repository.Employee.GetByIdAsync(Id);
                if (result == null) return NotFound(new ApiResponse(404));

                return Ok(new ApiOkResponse<EmployeeDTO>(_mapper.Map<EmployeeDTO>(result)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500));
            }
        }

        [HttpPut("{Id}")]
        [SwaggerOperation(
            Summary = "Update Employee Information",
            Description = "Modify an Employee's information data and saves changes to the database",
            OperationId = "UpdateEmployee"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<EmployeeDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> Put(int Id, [FromBody] EmployeeDTO value)
        {
            if (Id != value.Id) return BadRequest(new ApiResponse(400));
            try
            {
                var validation = new EmployeeValidator().Validate(value);
                if (!validation.IsValid)
                {
                    validation.Errors.ForEach(x =>
                    {
                        ModelState.AddModelError(x.PropertyName, x.ErrorMessage);
                    });
                    return BadRequest(new ApiBadRequestResponse(ModelState));
                }
                _repository.Employee.Update(_mapper.Map<Employee>(value));
                await _repository.SaveAsync();
                //_logger.LogInfo($"Update Arealist: {JsonSerializer.Serialize(value)} Successful");
                return Ok(new ApiOkResponse<EmployeeDTO>(value));
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                return BadRequest(new ApiResponse(500));

            }
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Delete Employee",
            Description = "Removes an Employee database on Id and saves changes to the database",
            OperationId = "DeleteEmployee"
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> Delete(int Id)
        {
            try
            {
                var result = await _repository.Employee.GetByIdAsync(Id);
                if (result == null) return BadRequest(new ApiResponse(404));

                _repository.Employee.Delete(result);

                await _repository.SaveAsync();
                //_logger.LogInfo($"Delete Arealist: {JsonSerializer.Serialize(result)} Successful.");
                return NoContent();
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                return BadRequest(new ApiResponse(500));
            }
        }
    }
}
