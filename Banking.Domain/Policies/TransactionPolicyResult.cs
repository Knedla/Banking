namespace Banking.Domain.Policies;

public class TransactionPolicyResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private TransactionPolicyResult(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static TransactionPolicyResult Success() => new(true);

    public static TransactionPolicyResult Failure(string errorMessage) =>
        new(false, errorMessage);

    public override string ToString()
    {
        return IsSuccess ? "Success" : $"Failure: {ErrorMessage}";
    }

    public static TransactionPolicyResult Combine(params TransactionPolicyResult[] results)
    {
        foreach (var result in results)
        {
            if (!result.IsSuccess)
                return result;
        }

        return Success();
    }
}
