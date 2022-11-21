namespace CurrencyConverter.Domain
{
    public interface ICurrencyVerifier
    {
        bool Verify(string currency);
    }
}