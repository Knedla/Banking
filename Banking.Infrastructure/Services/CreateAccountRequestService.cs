using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class CreateAccountRequestService : ICreateAccountRequestService
{
    private readonly ICreateAccountService _createAccountService;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IStateTransitionValidator<WorkItemStatus> _workItemStatusTransitionValidator;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IDepositService _depositService;

    public CreateAccountRequestService(
        ICreateAccountService createAccountService,
        IWorkItemRepository workItemRepository,
        IStateTransitionValidator<WorkItemStatus> workItemStatusTransitionValidator,
        IDomainEventDispatcher domainEventDispatcher,
        IDepositService depositService)
    {
        _createAccountService = createAccountService;
        _workItemRepository = workItemRepository;
        _workItemStatusTransitionValidator = workItemStatusTransitionValidator;
        _domainEventDispatcher = domainEventDispatcher;
        _depositService = depositService;
    }

    public async Task<CreateAccountRequestResponse> CreateWorkItemAsync(CreateAccountRequestRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var createdAt = DateTime.UtcNow;
        
        var workItem = new Domain.Entities.WorkItems.CreateAccountRequest()
        { 
            Id = Guid.NewGuid(),
            Status = WorkItemStatus.Pending,
            CreatedAt = createdAt,
            CreatedByUserId = request.UserId,
            LastModifiedAt = createdAt,
            LastModifiedByUserId = request.UserId,

            InvolvedPartyId = request.InvolvedPartyId,
            AccountType = request.AccountType,
            FromCurrencyCode = request.FromCurrencyCode,
            ToCurrencyCode = request.ToCurrencyCode,
            InitialDeposit = request.InitialDeposit,
            TransactionChannel = request.TransactionChannel
        };

        await _workItemRepository.AddAsync(workItem);

        var evt = new CreateAccountRequestAddedEvent(workItem);
        await _domainEventDispatcher.RaiseAsync(evt);

        if (workItem.AppliedRules != null && workItem.AppliedRules.Count > 0)
        {
            var response = new CreateAccountRequestResponse();
            response.ContinueExecution = false;
            response.AddInfo("Waiting for Create Account Request Work Item approval.");
            return response;
        }
        else
        {
            // TODO: make work item approval line; trigger workItemApproved in the end ?

            if (!_workItemStatusTransitionValidator.IsValidTransition(workItem.Status, WorkItemStatus.Approved)) // TODO: bad implementation; add somwhere to pipeline
                throw new Exception($"Cannot approved work item.");
            
            await _workItemRepository.UpdateStatusAsync(workItem, WorkItemStatus.Approved); // TODO: bad implementation; add approving somwhere to pipeline
        }

        var createAccountResponse = await _createAccountService.CreateAccountAsync(new CreateAccountRequest() { WorkItem = workItem, UserId = request.UserId });

        if (request.InitialDeposit > 0)
        {
            await _depositService.DepositAsync(new DepositRequest()
            {
                UserId = request.UserId,
                InvolvedPartyId = workItem.InvolvedPartyId,
                AccountId = createAccountResponse.AccountId,
                FromCurrencyCode = workItem.FromCurrencyCode,
                ToCurrencyCode = workItem.ToCurrencyCode,
                Amount = workItem.InitialDeposit,
                TransactionChannel = workItem.TransactionChannel
            });
        }

        return new CreateAccountRequestResponse // maybe add DepositRequest result ...
        {
            WorkItemId = workItem.Id,
            WorkItemStatus = workItem.Status
        };
    }
}
