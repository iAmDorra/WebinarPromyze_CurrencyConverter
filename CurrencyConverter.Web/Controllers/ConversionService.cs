using CurrencyConverter.Domain;
using CurrencyConverter.Infrastructure;

namespace CurrencyConverter.Web.Controllers
{
    public class ConversionService
    {
        public string Convert(string amountValue, string currencyName)
        {
            var converter = new Converter(new Rates(), new CurrencyVerifier(), new Logger());
            decimal amount = decimal.Parse(amountValue);
            var eurCurrency = new Currency("EUR");
            Currency targetCurrency = new Currency(currencyName);
            var convertedAmount = converter.Convert(amount, eurCurrency, targetCurrency);

            return convertedAmount.ToString();
        }
    }
}
