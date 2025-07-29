namespace Banking.Application.Notifications.Interfaces;

public interface ITemplateRenderer
{
    string Render(string template, object model);
}
