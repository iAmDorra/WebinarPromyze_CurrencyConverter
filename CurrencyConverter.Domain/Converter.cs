using System;

namespace CurrencyConverter.Domain
{
    public class Converter
    {
        private readonly IRates _rates;
        private readonly ICurrencyVerifier currencyVerifier;

        public Converter(IRates rates, ICurrencyVerifier currencyVerifier)
        {
            _rates = rates;
            this.currencyVerifier = currencyVerifier;
        }

        public decimal Convert(decimal amount, string sourceCurrency, string targetCurrency)
        {
            if (currencyVerifier.Verify(sourceCurrency))
            {
                if (currencyVerifier.Verify(targetCurrency))
                {
                    decimal conversionRate = _rates.GetRateOf(sourceCurrency, targetCurrency);
                    if (sourceCurrency.Equals(targetCurrency))
                    {
                        return amount;
                    }

                    var convertedValue = amount * conversionRate;
                    return convertedValue;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
