namespace Banking.Domain.Interfaces.StateMachine;

public interface IStateMachine<TState> where TState : Enum
{
    TState Current { get; }

    bool CanTransitionTo(TState next);
    void TransitionTo(TState next);
}
