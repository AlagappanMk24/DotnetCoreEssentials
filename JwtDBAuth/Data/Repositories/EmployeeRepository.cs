using JwtAuthDB.Data.Context;
using JwtAuthDB.Data.Repositories.Interface;
using JwtAuthDB.Entities;

namespace JwtAuthDB.Data.Repositories
{
    public class EmployeeRepository(JwtContext context) : IEmployeeRepository
    {
        private readonly JwtContext _context = context;
        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }
        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.SingleOrDefault(e => e.Id == id);
        }
        public Employee AddEmployee(Employee employee)
        {
            var result = _context.Employees.Add(employee);
            _context.SaveChanges();
            return result.Entity;
        }
        public Employee UpdateEmployee(Employee employee)
        {
            var result = _context.Employees.Update(employee);
            _context.SaveChanges();
            return result.Entity;
        }
        public bool DeleteEmployee(int id)
        {
            var employee = _context.Employees.SingleOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return true;
        }
    }
}
