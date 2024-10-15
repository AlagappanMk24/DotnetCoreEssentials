using JwtAuthDB.Entities;

namespace JwtAuthDB.Services.Interface
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployeeDetails();
        Employee GetEmployeeDetails(int id);
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        bool DeleteEmployee(int id);
    }
}
