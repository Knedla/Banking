using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Responses;

public class DepositResponse : BaseResponse
{
    public TransactionStatus TransactionStatus { get; set; }
}
