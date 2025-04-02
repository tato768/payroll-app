namespace Api.Calculator;

// Configuration of the payment calculator, can be sotred outside
public class PaycheckConfiguration
{
    public int PaychecksPerYear { get; } = 26;
    public decimal EmployeeBaseCost { get; } = 1000;
    public decimal PerDependentCost { get; } = 600;
    public decimal HighSalaryLimit { get; } = 80000;
    public decimal HighSalaryCostPercentage { get; } = 0.02m;
    public int DependentAgeLimit { get; } = 50;
    public decimal DependentAgeCost { get; } = 200;
}