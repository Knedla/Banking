using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Contacts;
using Banking.Domain.Entities.Parties;
using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Stores;

public class InMemoryBankingDataStore : IBankingDataStore // TODO: change with notmal data context -> this one is very poorly implemented
{
    private readonly Dictionary<Type, object> _dataSets;

    private List<WorkItem> _workItem;
    private List<Individual> _individuals;
    private List<Account> _accounts;

    public List<WorkItem> WorkItems => _workItem;
    public List<Individual> Individuals => _individuals;
    public List<Account> Accounts => _accounts;

    private List<Account> _accountsBackup;  ///////////////////////////////////////////////////////////////

    public InMemoryBankingDataStore()
    {
        InitLists();
        InitData();

        _dataSets = new()
        {
            { typeof(WorkItem), _workItem },
            { typeof(Individual), _individuals },
            { typeof(Account), _accounts }
        };
    }

    void InitLists()
    {
        _workItem = new List<WorkItem>();
        _individuals = new List<Individual>();
        _accounts = new List<Account>();
    }

    void InitData()
    {
        Guid individual1 = new Guid("f4bd0b90-10c6-47ff-bb35-113c0c779c3f");
        Guid individual2 = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6");

        var emailContactPurpose = ContactPurpose.Primary | ContactPurpose.Billing;

        Individuals.Add(
            new Individual()
            {
                Id = individual1,
                FirstName = "Igor",
                LastName = "Simovic",
                Name = "Igor Simovic",
                DateOfBirth = new DateTime(1990, 1, 1),
                Type = InvolvedPartyType.Individual,
                IdentificationDocuments = new List<IdentificationDocument>()
                {
                    new IdentificationDocument()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual1,
                        Type = IdentificationDocumentType.Passport,
                        DocumentNumber = "123456",
                        IssueDate = new DateTime(2020, 1, 1),
                        ExpirationDate = new DateTime(2030, 1, 1),
                        IssuingCountry = "Srb",
                        CreatedAt = DateTime.UtcNow,
                        IsVerified = true
                    }
                },
                Emails = new List<EmailAddress>()
                {
                    new EmailAddress()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual1,
                        Email = "1individual1@a.a",
                        Type = EmailType.Personal,
                        Purposes = emailContactPurpose
                    }
                },
                PhoneNumbers = new List<PhoneNumber>()
                {
                    new PhoneNumber()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual1,
                        CountryCode = "+381",
                        Number = "123456",
                        Type = PhoneType.Mobile,
                        Purposes = emailContactPurpose
                    }
                }
            });
        Individuals.Add(
            new Individual()
            {
                Id = individual2,
                FirstName = "Lena",
                LastName = "Markovic",
                Name = "Lena Markovic",
                DateOfBirth = new DateTime(2002, 2, 2),
                Type = InvolvedPartyType.Individual,
                IdentificationDocuments = new List<IdentificationDocument>()
                {
                    new IdentificationDocument()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual2,
                        Type = IdentificationDocumentType.DriverLicense,
                        DocumentNumber = "6786954",
                        IssueDate = new DateTime(2025, 2, 2),
                        ExpirationDate = new DateTime(2035, 2, 2),
                        IssuingCountry = "Xyz",
                        CreatedAt = DateTime.UtcNow,
                        IsVerified = true
                    }
                },
                Emails = new List<EmailAddress>()
                {
                    new EmailAddress()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual2,
                        Email = "2individual2@a.a",
                        Type = EmailType.Work,
                        Purposes = emailContactPurpose
                    }
                },
                PhoneNumbers = new List<PhoneNumber>()
                {
                    new PhoneNumber()
                    {
                        Id = Guid.NewGuid(),
                        InvolvedPartyId = individual2,
                        CountryCode = "+234",
                        Number = "987654",
                        Type = PhoneType.Mobile,
                        Purposes = emailContactPurpose
                    }
                }
            });

        Guid account1 = new Guid("f6d6cde9-3e0e-4a7a-9081-efb972f9d0b2");

        Accounts.Add(
            new Account()
            {
                Id = account1,
                InvolvedPartyId = new Guid("9ba7d2a3-6a9e-4e78-93a0-42f3d5ec8ef6"),
                AccountNumber = "123",
                AccountType = AccountType.Standard,
                CreatedAt = DateTime.UtcNow,
                Balances = new AccountBalance[]
                {
                    new AccountBalance()
                    {
                        AccountId = account1,
                        CurrencyCode = "EUR",
                        Balance = 775533,
                        AvailableBalance = 775533,
                    }
                }
            });
    }

    public List<T> Get<T>() where T : class, IEntity
    {
        if (_dataSets.TryGetValue(typeof(T), out var list))
            return list as List<T> ?? throw new InvalidCastException();

        throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}");
    }

    public Task BeginTransactionAsync()
    {
        _accountsBackup = _accounts.Select(a => new Account
        {
            Id = a.Id,
            AccountNumber = a.AccountNumber,
            //Balance = a.Balance
        }).ToList();

        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        _accountsBackup = null;
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        _accounts = _accountsBackup;

        return Task.CompletedTask;
    }

    // TODO: implement
    public Task SaveAsync() => throw new NotImplementedException();
    public Task CreateSavepointAsync() => throw new NotImplementedException();
    public Task RollbackToSavepointAsync() => throw new NotImplementedException();
    public Task ReleaseSavepointAsync() => throw new NotImplementedException();
}
