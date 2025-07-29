using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class CreateAccountService : ICreateAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IAccountNumberGenerator _accountNumberGenerator;
    private readonly IAccountBalanceFactory _accountBalanceFactory;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CreateAccountService(
        IAccountRepository accountRepository,
        IWorkItemRepository workItemRepository,
        IAccountNumberGenerator accountNumberGenerator,
        IAccountBalanceFactory accountBalanceFactory,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _accountRepository = accountRepository;
        _workItemRepository = workItemRepository;
        _accountNumberGenerator = accountNumberGenerator;
        _accountBalanceFactory = accountBalanceFactory;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var workItem = request.WorkItem;

        if (workItem == null && request.WorkItemId != Guid.Empty)
            workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId);

        if (workItem == null)
            throw new Exception($"Cannot find work item.");

        var createAccountRequest = workItem as Domain.Entities.WorkItems.CreateAccountRequest;

        if (createAccountRequest == null)
            throw new Exception($"Cannot cast to CreateAccountRequest.");

        var createdAt = DateTime.UtcNow;
        var accountId = Guid.NewGuid();
        var accountNumber = await _accountNumberGenerator.GenerateAsync(createAccountRequest.AccountType);
        var balances = await _accountBalanceFactory.CreateInitialBalancesAsync(
            new AccountBalanceInitializationContext()
            {
                AccountId = accountId, 
                AccountType = createAccountRequest.AccountType, 
            });
        
        var account = new Account
        {
            Id = accountId,
            InvolvedPartyId = createAccountRequest.InvolvedPartyId,
            AccountNumber = accountNumber,
            AccountType = createAccountRequest.AccountType,
            CreatedAt = createdAt,
            CreatedByUserId = request.UserId,
            LastModifiedAt = createdAt,
            LastModifiedByUserId = request.UserId,
            Balances = balances
        };

        await _accountRepository.AddAsync(account);

        return new CreateAccountResponse
        {
            AccountId = accountId
        };

        // TODO: trigger AccountCreated event if needed
    }
}
