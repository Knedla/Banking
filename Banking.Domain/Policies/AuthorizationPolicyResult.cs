namespace Banking.Domain.Policies;

public class AuthorizationPolicyResult // same as TransactionPolicyResult; make a base PolicyResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private AuthorizationPolicyResult(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static AuthorizationPolicyResult Success() => new(true);

    public static AuthorizationPolicyResult Failure(string errorMessage) =>
        new(false, errorMessage);

    public override string ToString()
    {
        return IsSuccess ? "Success" : $"Failure: {ErrorMessage}";
    }

    public static AuthorizationPolicyResult Combine(params AuthorizationPolicyResult[] results)
    {
        foreach (var result in results)
        {
            if (!result.IsSuccess)
                return result;
        }

        return Success();
    }
}
