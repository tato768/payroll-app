using Api.Models;

namespace Api.Repositories;

public interface IDependentRepository
{
    Task<List<Dependent>> GetAll();
    Task<Dependent?> GetById(int id);    
}