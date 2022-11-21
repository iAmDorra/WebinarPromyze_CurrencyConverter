namespace CurrencyConverter.Domain
{
    public interface IRates
    {
        decimal GetRateOf(string currency, string targetCurrency);
    }
}