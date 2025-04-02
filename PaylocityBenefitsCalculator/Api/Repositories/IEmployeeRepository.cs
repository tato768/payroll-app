using Api.Models;

namespace Api.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAll();
    Task<Employee?> GetById(int id);
}