using JwtAuthDB.Entities;
using JwtAuthDB.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;
        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            try
            {
                var employees = _employeeService.GetEmployeeDetails();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred while fetching employees.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> Get(int id)
        {
            try
            {
                var employee = _employeeService.GetEmployeeDetails(id);
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, $"An error occurred while fetching employee with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult<Employee> Post([FromBody] Employee employee)
        {
            try
            {
                var createdEmployee = _employeeService.AddEmployee(employee);
                return CreatedAtAction(nameof(Get), new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred while creating the employee");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Employee> Put(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return BadRequest("Employee ID mismatch");
                }

                var updatedEmployee = _employeeService.UpdateEmployee(employee);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, $"An error occurred while updating the employee with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var isDeleted = _employeeService.DeleteEmployee(id);
                if (!isDeleted)
                {
                    return NotFound("Employee not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, $"An error occurred while deleting the employee with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}