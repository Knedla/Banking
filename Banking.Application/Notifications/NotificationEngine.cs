using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Notifications.Interfaces;
using Banking.Application.Notifications.Routing;
using Banking.Domain.Interfaces;

namespace Banking.Application.Notifications;

public class NotificationEngine<TEvent> : INotificationEngine<TEvent> where TEvent : IDomainEvent
{
    private readonly INotificationRuleProvider _ruleProvider;
    private readonly INotificationChannelDispatcher _channelDispatcher;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IInvolvedPartyService _involvedPartyService;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly INotificationDestinationResolver _notificationDestinationResolver;

    public NotificationEngine(
        INotificationRuleProvider ruleProvider,
        INotificationChannelDispatcher channelDispatcher,
        ITemplateRenderer templateRenderer,
        IInvolvedPartyService involvedPartyService,
        IExpressionEvaluator expressionEvaluator,
        INotificationDestinationResolver notificationDestinationResolver)
    {
        _ruleProvider = ruleProvider;
        _channelDispatcher = channelDispatcher;
        _templateRenderer = templateRenderer;
        _involvedPartyService = involvedPartyService;
        _expressionEvaluator = expressionEvaluator;
        _notificationDestinationResolver = notificationDestinationResolver;
    }

    // refactor needed
    // current logic is to send a notification only to the person who initiated the transaction
    // it should send a notification to everyone who is subscribed to it on the account
    public async Task HandleEventAsync(TEvent domainEvent)
    {
        var involvedPartyResponse = await _involvedPartyService.GetInvolvedPartyAsync(new InvolvedPartyRequest() { InvolvedPartyId = domainEvent.InvolvedPartyId });

        if (involvedPartyResponse == null)
            return;

        var rules = await _ruleProvider.GetEffectiveRulesAsync(domainEvent, involvedPartyResponse.InvolvedParty.NotificationOverride);

        foreach (var rule in rules)
        {
            // Use dynamic expression evaluator (e.g., RulesEngine or similar)
            if (!_expressionEvaluator.Evaluate(rule.Condition, domainEvent))
                continue;

            var message = _templateRenderer.Render(rule.MessageTemplate, domainEvent);

            foreach (var channel in rule.Channels)
            {
                List<string> destination = await _notificationDestinationResolver.ResolveAsync(channel, involvedPartyResponse.InvolvedParty);
                await _channelDispatcher.DispatchAsync(channel, domainEvent.InvolvedPartyId, destination, message, rule.Priority);
            }
        }
    }
}
