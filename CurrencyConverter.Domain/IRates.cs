namespace CurrencyConverter.Domain
{
    public interface IRates
    {
        decimal GetRateOf(Currency currency, Currency targetCurrency);
    }
}