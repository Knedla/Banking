using Banking.Domain.Enumerations;

namespace Banking.Application.Models.Responses;

public class TransferResponse : BaseResponse
{
    public TransactionStatus TransactionStatus { get; set; }
}
