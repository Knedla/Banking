namespace Banking.Domain.Configuration;

public class TransactionFeeSettings
{
    public List<FeeValue> MinimumBalanceThreshold { get; set; }
    public List<FeeValue> MinimumBalanceFee { get; set; }
    public List<FeeValue> InternationalTransferFee { get; set; } // TODO: split into incoming and outgoing
    public List<FeeValue> StandardTransferFee { get; set; }
    public Dictionary<string, List<FeeValue>> ChannelFees { get; set; } // TODO: refactor; prone to string errors
    public List<FeeValue> NonSameBankWithdrawal { get; set; }
    public string BankIdentifier { get; set; }
    public string DefaultCurrencyCode { get; set; }
    public string DefaultAccountNumber { get; set; }
}
