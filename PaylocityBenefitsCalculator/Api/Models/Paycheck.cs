namespace Api.Models;

public class Paycheck
{
    public int EmployeeId { get; set; }
    public int Year { get; set; }
    public int PaychechNumber { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal SalaryToPay { get; set; }
    public decimal TotalBenefitCost { get; set; }
    public List<BenefitCostExplain> BenefitCosts { get; set; } = new List<BenefitCostExplain>();
}