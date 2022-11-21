using CurrencyConverter.Domain;
using CurrencyConverter.Infrastructure;

namespace CurrencyConverter.Web.Controllers
{
    public class ConversionService
    {
        public string Convert(string amountValue, string currencyName)
        {
            var converter = new Converter(new Rates(), new CurrencyVerifier());
            decimal amount = decimal.Parse(amountValue);
            var eurCurrency = "EUR";
            var convertedAmount = converter.Convert(amount, eurCurrency, currencyName);

            return convertedAmount.ToString();
        }
    }
}
