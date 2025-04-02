using Api.Models;

namespace Api.Calculator;

public interface IPaycheckCalculator
{
    Paycheck GetPaycheck(Employee employee, int year, int paycheckNumber);
}