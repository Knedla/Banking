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
}
