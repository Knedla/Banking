namespace Banking.Services.Interfaces
{
    public interface ITelemetryService // implement this ...
    {
        void TrackEvent(string name, IDictionary<string, object>? properties = null);
        void TrackDuration(string name, TimeSpan duration);
    }
}
