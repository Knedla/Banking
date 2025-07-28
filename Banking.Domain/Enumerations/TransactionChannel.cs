namespace Banking.Domain.Enumerations;

public enum TransactionChannel
{
    MobileApp,
    WebPortal,
    ATM,
    POS,            // Transaction initiated via a Point-of-Sale (POS) terminal. Used for in-store purchases using debit or credit cards.
    Branch,
    CallCenter,     // Transaction initiated by a bank employee via phone call or customer support. Often verified with additional security steps.
    InternalSystem,
    Api,            // Transaction initiated via an API integration. Typically used by third-party services like payroll systems, fintech apps, or ERP systems.
    Scheduled,
    StandingOrder,  // Pre-authorized, recurring user-defined instruction. E.g., standing orders for rent, tuition, or regular transfers.
    Cheque,
    Swift,          // Incoming or outgoing international transaction via the SWIFT network. Typically includes additional data like BIC, MT messages, and reference codes.
    Sepa,           // Eurozone electronic transfer processed through the SEPA network. Fast and low-cost transfers in EUR within the EU/EEA.
    AdminPanel,
    OtherBank       // Transaction initiated by another bank or payment processor, such as an incoming payment to a user's account.
}
