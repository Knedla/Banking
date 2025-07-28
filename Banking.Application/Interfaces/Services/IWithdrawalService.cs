using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Interfaces.Services;

public interface IWithdrawalService
{
    Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request);
}
