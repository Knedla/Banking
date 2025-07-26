using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services
{
    public class CreateAccountService : ICreateAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                //OwnerName = request.OwnerName,
                //AccountType = request.AccountType,
                Balance = request.InitialDeposit
            };

            await _accountRepository.AddAsync(account);

            return new CreateAccountResponse
            {
                AccountId = account.Id,
                AccountNumber = account.Id.ToString(),
                InitialBalance = account.Balance
            };
        }
    }
}
