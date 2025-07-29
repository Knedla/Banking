namespace Banking.Application.Interfaces;

public interface IRuleProcessor
{
    Task ApplyRulesAsync<T>(T context) where T : class;
}
