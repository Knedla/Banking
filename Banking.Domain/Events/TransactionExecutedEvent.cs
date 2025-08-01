﻿using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public record TransactionExecutedEvent(
    Guid TransactionId,
    Guid InvolvedPartyId
) : IDomainEvent;