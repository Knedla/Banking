namespace Banking.Application.Common.Expressions;

public interface IExpressionEvaluator
{
    bool Evaluate(string expression, object context);
}