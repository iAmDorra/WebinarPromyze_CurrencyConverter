namespace CurrencyConverter.Domain
{
    public interface ICurrencyVerifier
    {
        bool Verify(Currency currency);
    }
}