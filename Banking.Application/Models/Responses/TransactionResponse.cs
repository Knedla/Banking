using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Responses;

public class TransactionResponse : BaseResponse
{
    public TransactionStatus TransactionStatus { get; set; }
}
