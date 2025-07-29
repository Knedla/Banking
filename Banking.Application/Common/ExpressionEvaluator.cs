using Banking.Application.Interfaces;
using Banking.Domain.Enumerations;
using DynamicExpresso;

namespace Banking.Application.Common;

public class ExpressionEvaluator : IExpressionEvaluator // TODO: change it to async
{
    private readonly Interpreter _interpreter;

    public ExpressionEvaluator()
    {
        _interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);
    }

    public bool Evaluate(string condition, object context)
    {
        if (string.IsNullOrWhiteSpace(condition))
            return true;

        _interpreter.Reference(typeof(AccountType));

        // register context's properties as variables
        foreach (var prop in context.GetType().GetProperties())
        {
            var value = prop.GetValue(context);
            _interpreter.SetVariable(prop.Name, value, prop.PropertyType);
        }

        try
        {
            return _interpreter.Eval<bool>(condition);
        }
        catch (Exception ex)
        {
            // Optional: log warning
            return false;
        }
    }

    /*public bool Evaluate(string expression, object context)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return true;

        try
        {
            var interpreter = new Interpreter()
                .Reference(typeof(AccountType))
                .Reference(typeof(AccountTier));

            // Register each property of the context as a variable
            foreach (var prop in context.GetType().GetProperties())
            {
                var value = prop.GetValue(context);
                interpreter.SetVariable(prop.Name, value, prop.PropertyType);
            }

            // Evaluate expression directly using those variables
            return interpreter.Eval<bool>(expression);
        }
        catch (Exception ex)
        {
            // Optionally log this
            // _logger.LogWarning(ex, $"Invalid expression: {expression}");
            return false;
        }
    }*/

    /*public bool Evaluate(string expression, object context)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return true;

        try
        {
            // Register all properties of the context as variables
            foreach (var prop in context.GetType().GetProperties())
            {
                var value = prop.GetValue(context);
                _interpreter.SetVariable(prop.Name, value, prop.PropertyType);
            }

            // Evaluate the expression directly (no need to prefix with "context.")
            return _interpreter.Eval<bool>(expression);

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
    }*/
}
