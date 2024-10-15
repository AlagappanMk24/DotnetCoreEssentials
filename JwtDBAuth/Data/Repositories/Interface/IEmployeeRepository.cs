using JwtAuthDB.Entities;

namespace JwtAuthDB.Data.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(int id);
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        bool DeleteEmployee(int id);
    }
}
