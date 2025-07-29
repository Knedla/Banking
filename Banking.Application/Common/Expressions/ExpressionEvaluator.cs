using DynamicExpresso;

namespace Banking.Application.Common.Expressions;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly Interpreter _interpreter;

    public ExpressionEvaluator()
    {
        _interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);
    }

    public bool Evaluate(string expression, object context)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return true;

        try
        {
            // Bind the domain event as "event"
            _interpreter.SetVariable("event", context, context.GetType());

            var lambda = _interpreter.Parse(expression);
            var result = lambda.Invoke();

            return result is bool b && b;
        }
        catch (Exception ex)
        {
            // Optionally log the error here
            // e.g. _logger.LogWarning(ex, $"Invalid expression: {expression}");
            return false;
        }
    }
}
