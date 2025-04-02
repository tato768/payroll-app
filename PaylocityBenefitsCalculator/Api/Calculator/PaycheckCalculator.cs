using Api.Models;

namespace Api.Calculator;

public class PaycheckCalculator : IPaycheckCalculator
{
    private readonly PaycheckConfiguration configuration;
    private readonly List<IBenefitCostApplier> benefitCostAppliers;

    public PaycheckCalculator(PaycheckConfiguration configuration)
    {
        this.configuration = configuration;
        this.benefitCostAppliers = PrepareBenefitCostAppliers(configuration);
    }

    public Paycheck GetPaycheck(Employee employee, int year, int paycheckNumber)
    {
        // This is very simplistic, year and paycheck number is ignored, it jsut seemd like useful info in the begining...
        // per paycheck cost = 12 * monthly cost / paychecks per year
        // This is certainly not correct and falls apart on boundaries, like end of the year
        var baseSalary = GetBaseSalaryPerPaycheck(employee);
        var benefitCosts = GetBenefitCosts(employee);
        var totalBenefitCost = benefitCosts.Sum(x => x.Amount);

        return new Paycheck
        {
            EmployeeId = employee.Id,
            Year = year,
            PaychechNumber = paycheckNumber,
            BaseSalary = baseSalary,
            TotalBenefitCost = totalBenefitCost,
            SalaryToPay = baseSalary - totalBenefitCost,
            BenefitCosts = benefitCosts
        };
    }

    private decimal GetBaseSalaryPerPaycheck(Employee employee)
    {
        return employee.Salary / configuration.PaychecksPerYear;
    }

    private List<BenefitCostExplain> GetBenefitCosts(Employee employee)
    {
        return benefitCostAppliers
            .Where(applier => applier.IsApplicable(employee))
            .SelectMany(applier => applier.ApplyBenefitCost(employee))
            .ToList();
    }

    private static List<IBenefitCostApplier> PrepareBenefitCostAppliers(PaycheckConfiguration configuration)
    {
        // Chain of responsibility, sort of, seems fitting in tis scenario, maybe there is no need to have two methods
        // TODO: Calculating the per paycheck cost here is ugly, maybe this should be pushed down to constructors, but I don't like that either
        return new List<IBenefitCostApplier>
        {
            new EmployeeBaseCost(12 * configuration.EmployeeBaseCost / configuration.PaychecksPerYear),
            new PerDependentCost(12 * configuration.PerDependentCost / configuration.PaychecksPerYear),
            new HighSalaryCost(configuration.HighSalaryLimit, configuration.HighSalaryCostPercentage, configuration.PaychecksPerYear),
            new DependandAgeCost(configuration.DependentAgeLimit, 12 * configuration.DependentAgeCost / configuration.PaychecksPerYear),
        };
    }
}
