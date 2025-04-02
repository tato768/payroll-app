using Api.Models;

namespace Api.Repositories;

public class DependentRepository : IDependentRepository
{
    public Task<List<Dependent>> GetAll()
    {
        var dependents = GetDependents().ToList();
        return Task.FromResult(dependents);
    }

    public Task<Dependent?> GetById(int id)
    {
        var dependent = GetDependents().FirstOrDefault(x => x.Id == id);
        return Task.FromResult(dependent);
    }

    private static IEnumerable<Dependent> GetDependents() => Database.Employees.SelectMany(x => x.Dependents);
}