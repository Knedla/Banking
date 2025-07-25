namespace Banking.Core.Enumerations
{
    // do not forget to update PhaseInterfaceHelper
    public enum CommandPhaseType
    {
        Initialization = 0,
        Validation = 1,
        PreExecution = 2,
        Execution = 3, // required
        PostExecution = 4
    }
}
