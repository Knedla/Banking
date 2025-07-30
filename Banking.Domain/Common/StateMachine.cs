using Banking.Domain.Interfaces.StateMachine;

namespace Banking.Domain.Common;

public class StateMachine<TState> : IStateMachine<TState> where TState : Enum
{
    private readonly IStateValidator<TState> _validator;

    public TState Current { get; private set; }

    public StateMachine(TState initial, IStateValidator<TState> validator)
    {
        Current = initial;
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public bool CanTransitionTo(TState next)
    {
        return _validator.IsValidTransition(Current, next);
    }

    public void TransitionTo(TState next)
    {
        if (!CanTransitionTo(next))
            throw new InvalidOperationException($"Invalid transition from {Current} to {next}");

        Current = next;
    }
}
