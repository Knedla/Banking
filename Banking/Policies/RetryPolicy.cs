using Banking.Attributes;
using System.Reflection;

namespace Banking.Policies
{
    public record RetryPolicy(int MaxRetries, TimeSpan Delay, bool Exponential);

    public static class RetryPolicyResolver
    {
        public static RetryPolicy? GetPolicy(Type type)
        {
            var attr = type.GetCustomAttribute<RetryAttribute>();
            return attr == null ? null : new RetryPolicy(attr.MaxRetries, TimeSpan.FromSeconds(attr.DelaySeconds), attr.UseExponentialBackoff);
        }
    }
}
