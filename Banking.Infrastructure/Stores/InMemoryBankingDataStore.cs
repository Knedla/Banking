﻿using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Contacts;
using Banking.Domain.Entities.Parties;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Stores;

public class InMemoryBankingDataStore : IBankingDataStore // TODO: change with notmal data context -> this one is very poorly implemented
{
    private readonly Dictionary<Type, object> _dataSets;

    private List<WorkItem> _workItem;
    private List<InvolvedParty> _involvedParties;
    private List<Individual> _individuals;
    private List<Account> _accounts;
    private List<Transaction> _transactions;

    public List<WorkItem> WorkItems => _workItem;
    public List<InvolvedParty> InvolvedParties => _involvedParties;
    public List<Individual> Individuals => _individuals;
    public List<Account> Accounts => _accounts;
    public List<Transaction> Transactions => _transactions;

    public InMemoryBankingDataStore()
    {
        InitLists();
        InitData();

        _dataSets = new()
        {
            { typeof(WorkItem), _workItem },
            { typeof(InvolvedParty), _involvedParties },
            { typeof(Individual), _individuals },
            { typeof(Account), _accounts },
            { typeof(Transaction), _transactions }
        };

        _involvedParties.AddRange(_individuals); // helper
    }

    void InitLists()
    {
        _workItem = new List<WorkItem>();
        _involvedParties = new List<InvolvedParty>();
        _individuals = new List<Individual>();
        _accounts = new List<Account>();
        _transactions = new List<Transaction>();
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
                PrimaryCurrencyCode = "EUR",
                CreatedAt = DateTime.UtcNow,
                Balances = new AccountBalance[]
                {
                    new AccountBalance()
                    {
                        AccountId = account1,
                        CurrencyCode = "EUR",
                        Balance = 3000,
                        AvailableBalance = 3000,
                    },
                    new AccountBalance()
                    {
                        AccountId = account1,
                        CurrencyCode = "USD",
                        Balance = 2000,
                        AvailableBalance = 2000,
                    }
                },
                Overdraft = new Overdraft()
                {
                    Limit = 1000,
                    CurrencyCode = "EUR"
                }
            });

        Guid account2 = new Guid("b67dc798-e95f-464a-bbd1-f56b57d60a5e");

        Accounts.Add(
            new Account()
            {
                Id = account2,
                InvolvedPartyId = new Guid("f4bd0b90-10c6-47ff-bb35-113c0c779c3f"),
                AccountNumber = "456",
                AccountType = AccountType.Standard,
                PrimaryCurrencyCode = "EUR",
                CreatedAt = DateTime.UtcNow,
                Balances = new AccountBalance[]
                {
                    new AccountBalance()
                    {
                        AccountId = account2,
                        CurrencyCode = "EUR",
                        Balance = 500,
                        AvailableBalance = 500,
                    }
                },
                Overdraft = new Overdraft()
                {
                    Limit = 1000,
                    CurrencyCode = "EUR"
                }
            });

        Guid account3 = new Guid("1a8df24e-0679-41f6-a53c-f37a0c114bb0"); // should simulate bank account for fees

        Accounts.Add(
            new Account()
            {
                Id = account3,
                InvolvedPartyId = new Guid("f4bd0b90-10c6-47ff-bb35-113c0c779c3f"),
                AccountNumber = "789",
                AccountType = AccountType.Standard,
                PrimaryCurrencyCode = "EUR",
                CreatedAt = DateTime.UtcNow,
                Balances = new AccountBalance[]
                {
                    new AccountBalance()
                    {
                        AccountId = account3,
                        CurrencyCode = "EUR",
                        Balance = 0,
                        AvailableBalance = 0,
                    },
                    new AccountBalance()
                    {
                        AccountId = account3,
                        CurrencyCode = "USD",
                        Balance = 0,
                        AvailableBalance = 0,
                    }
                },
                Overdraft = new Overdraft()
                {
                    Limit = 1000,
                    CurrencyCode = "EUR"
                }
            });
    }

    public List<T> Get<T>() where T : class, IEntity
    {
        if (_dataSets.TryGetValue(typeof(T), out var list))
            return list as List<T> ?? throw new InvalidCastException();

        throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}");
    }

    // TODO: implement
    public Task BeginTransactionAsync() => Task.CompletedTask; // => throw new NotImplementedException();
    public Task CommitAsync() => Task.CompletedTask; // => throw new NotImplementedException();
    public Task RollbackAsync() => Task.CompletedTask; // => throw new NotImplementedException();
    public Task SaveAsync() => Task.CompletedTask; // => throw new NotImplementedException();
    public Task CreateSavepointAsync() => Task.CompletedTask; //  => throw new NotImplementedException();
    public Task RollbackToSavepointAsync() => Task.CompletedTask; // => throw new NotImplementedException();
    public Task ReleaseSavepointAsync() => Task.CompletedTask; // => throw new NotImplementedException();
}
