using JwtAuthDB.Data;
using JwtAuthDB.Data.Repositories.Interface;
using JwtAuthDB.Entities;
using JwtAuthDB.Services.Interface;

namespace JwtAuthDB.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        public List<Employee> GetEmployeeDetails()
        {
            return _employeeRepository.GetAllEmployees();
        }
        public Employee GetEmployeeDetails(int id)
        {
            return _employeeRepository.GetEmployeeById(id);
        }
        public Employee AddEmployee(Employee employee)
        {
            return _employeeRepository.AddEmployee(employee);
        }
        public Employee UpdateEmployee(Employee employee)
        {
            return _employeeRepository.UpdateEmployee(employee);
        }
        public bool DeleteEmployee(int id)
        {
            return _employeeRepository.DeleteEmployee(id);
        }
    }
}
