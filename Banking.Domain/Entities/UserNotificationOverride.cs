using Banking.Application.Notifications.Rules;

namespace Banking.Domain.Entities;

public class InvolvedPartyNotificationOverride
{
    public Guid InvolvedPartyId { get; set; }

    // Match the event to override
    public string EventType { get; set; }

    // Optional override fields
    public string? Condition { get; set; }
    public List<string>? Channels { get; set; }
    public string? MessageTemplate { get; set; }

    public NotificationPriority? Priority { get; set; }

    // Optional toggle
    public bool? IsEnabled { get; set; }
}
