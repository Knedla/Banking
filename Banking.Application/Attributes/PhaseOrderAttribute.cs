namespace Banking.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PhaseOrderAttribute : Attribute
    {
        public int Order { get; }
        public PhaseOrderAttribute(int order) => Order = order;
    }
}
