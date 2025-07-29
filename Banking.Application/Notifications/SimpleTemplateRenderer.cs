using Banking.Application.Notifications.Interfaces;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Banking.Application.Notifications;

public class SimpleTemplateRenderer : ITemplateRenderer
{
    public string Render(string template, object model)
    {
        if (string.IsNullOrWhiteSpace(template) || model == null)
            return template;

        return Regex.Replace(template, @"{(\w+)}", match =>
        {
            var propName = match.Groups[1].Value;
            var prop = model.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            var value = prop?.GetValue(model);
            return value?.ToString() ?? match.Value;
        });
    }
}
