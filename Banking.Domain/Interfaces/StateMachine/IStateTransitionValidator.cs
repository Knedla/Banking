namespace Banking.Domain.Interfaces.StateMachine;

public interface IStateTransitionValidator<TState> where TState : Enum
{
    bool IsValidTransition(TState current, TState next);
}
