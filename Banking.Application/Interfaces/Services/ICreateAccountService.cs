using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Interfaces.Services
{
    public interface ICreateAccountService
    {
        Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request);
    }
}
