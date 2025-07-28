namespace Banking.Application.Interfaces.Services;

public interface ITelemetryService // TODO: implement this ...
{
    void TrackEvent(string name, IDictionary<string, object>? properties = null);
    void TrackDuration(string name, TimeSpan duration);
}
