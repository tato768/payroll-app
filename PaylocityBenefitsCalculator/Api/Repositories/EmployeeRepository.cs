using Api.Models;

namespace Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    public Task<List<Employee>> GetAll()
    {
        return Task.FromResult(Database.Employees);
    }

    public Task<Employee?> GetById(int id)
    {
        var employee = Database.Employees.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(employee);
    }
}