namespace Banking.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RetryAttribute : Attribute
    {
        public int MaxRetries { get; }
        public int DelaySeconds { get; }
        public bool UseExponentialBackoff { get; }

        public RetryAttribute(int maxRetries = 3, int delaySeconds = 2, bool useExponentialBackoff = false)
        {
            MaxRetries = maxRetries;
            DelaySeconds = delaySeconds;
            UseExponentialBackoff = useExponentialBackoff;
        }
    }
}
