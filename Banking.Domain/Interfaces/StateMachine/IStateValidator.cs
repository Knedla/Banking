namespace Banking.Domain.Interfaces.StateMachine;

public interface IStateValidator<TState> where TState : Enum
{
    bool IsValidTransition(TState current, TState next);
}
