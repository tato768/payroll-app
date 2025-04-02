namespace Api.Dtos.Paycheck;

public class BenefitCostExplainDto
{
    public decimal Amount { get; set;}
    public string Reason { get; set;} = string.Empty;
}