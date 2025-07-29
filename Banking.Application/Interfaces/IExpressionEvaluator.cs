namespace Banking.Application.Interfaces;

public interface IExpressionEvaluator // maybe rename to IRuleEvaluator ?
{
    bool Evaluate(string expression, object context);
}