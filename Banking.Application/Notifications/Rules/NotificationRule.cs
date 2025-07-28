namespace Banking.Application.Notifications.Rules;

public class NotificationRule
{
    public string EventType { get; set; } // e.g. "TransactionExecutedEvent"
    public string Condition { get; set; } // Optional expression like "Amount > 1000"
    public List<string> Channels { get; set; } = new(); // e.g. ["Email", "SMS"]
    public string MessageTemplate { get; set; } // e.g. "You sent {Amount} to {ReceiverName}"
    public NotificationPriority Priority { get; set; }
}
