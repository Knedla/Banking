using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Responses;

public class WithdrawalResponse : BaseResponse
{
    public TransactionStatus TransactionStatus { get; set; }
}
