using Api.Models;


namespace Api.Calculator;


public interface IBenefitCostApplier
{
    bool IsApplicable(Employee employee);
    List<BenefitCostExplain> ApplyBenefitCost(Employee employee);
}

//TODO: put into separate files

public class EmployeeBaseCost : IBenefitCostApplier
{
    private readonly decimal costPerPaycheck;

    public EmployeeBaseCost(decimal costPerPaycheck)
    {
        this.costPerPaycheck = costPerPaycheck;
    }

    public bool IsApplicable(Employee employee) => true;

    public List<BenefitCostExplain> ApplyBenefitCost(Employee employee)
    {
        return new List<BenefitCostExplain>
        {
            new BenefitCostExplain
            {
                Amount = costPerPaycheck,
                Reason = "Employee's base cost"
            }
        };
    }
}

public class PerDependentCost : IBenefitCostApplier
{
    private readonly decimal costPerPaycheck;

    public PerDependentCost(decimal costPerPaycheck)
    {
        this.costPerPaycheck = costPerPaycheck;
    }

    public bool IsApplicable(Employee employee) => employee.Dependents.Any();

    public List<BenefitCostExplain> ApplyBenefitCost(Employee employee)
    {
        return employee.Dependents
            .Select(x => new BenefitCostExplain{
                Amount = costPerPaycheck,
                Reason = $"Dependent cost for {x.FirstName} {x.LastName}"
            })
            .ToList();
    }
}

public class HighSalaryCost : IBenefitCostApplier
{
    private readonly decimal highSalaryLimit;
    private readonly decimal highSalaryCostPercentage;
    private readonly int paychecksPerYear;

    public HighSalaryCost(decimal highSalaryLimit, decimal highSalaryCostPercentage, int paychecksPerYear)
    {
        this.highSalaryLimit = highSalaryLimit;
        this.highSalaryCostPercentage = highSalaryCostPercentage;
        this.paychecksPerYear = paychecksPerYear;
    }

    public bool IsApplicable(Employee employee) => employee.Salary > highSalaryLimit;

    public List<BenefitCostExplain> ApplyBenefitCost(Employee employee)
    {
        var costPerPaycheck = employee.Salary * highSalaryCostPercentage / paychecksPerYear;
        return new List<BenefitCostExplain>
        {
            new BenefitCostExplain
            {
                Amount = costPerPaycheck,
                Reason = $"Employee's salary over {highSalaryLimit}"
            }
        };
    }
}

public class DependandAgeCost : IBenefitCostApplier
{
    private int dependentAgeLimit;
    private readonly decimal costPerPaycheck;

    public DependandAgeCost(int dependentAgeLimit, decimal costPerPaycheck)
    {
        this.dependentAgeLimit = dependentAgeLimit;
        this.costPerPaycheck = costPerPaycheck;
    }

    public bool IsApplicable(Employee employee) => employee.Dependents.Any(IsOlderThanLimit);

    public List<BenefitCostExplain> ApplyBenefitCost(Employee employee)
    {
        return employee.Dependents
            .Where(IsOlderThanLimit)
            .Select(x => new BenefitCostExplain{
                Amount = costPerPaycheck,
                Reason = $"Dependent {x.FirstName} {x.LastName} older than {dependentAgeLimit}"
            })
            .ToList();
    }

    private bool IsOlderThanLimit(Dependent dependent)
    {
        // This approach has many problems
        // - We completely ignore timezones and how dates are stored
        // - I would hide DateTime.Now behinf an interface to allow it to be mocked
        // - We execute this code multiple times and can get different results
        var dateOfBirthThreshold = DateTime.Now.AddYears(-dependentAgeLimit);
        return dependent.DateOfBirth < dateOfBirthThreshold;
    }
}