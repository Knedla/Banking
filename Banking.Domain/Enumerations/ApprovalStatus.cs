﻿namespace Banking.Domain.Enumerations;

public enum ApprovalStatus
{
    // maybe PoliciesTriggerRequired
    NotRequired = 0,
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}
