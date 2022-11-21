using System;

namespace CurrencyConverter.Domain
{
    public class Converter
    {
        private readonly IRates _rates;
        private readonly ICurrencyVerifier currencyVerifier;
        private ILogger logger;

        public Converter(IRates rates, ICurrencyVerifier currencyVerifier, ILogger logger)
        {
            _rates = rates;
            this.currencyVerifier = currencyVerifier;
            this.logger = logger;
        }

        public decimal Convert(decimal amount, string sourceCurrency, string targetCurrency)
        {
            if (currencyVerifier.Verify(sourceCurrency))
            {
                if (currencyVerifier.Verify(targetCurrency))
                {
                    if(amount < 0)
                    {
                        throw new InvalidOperationException();
                    }

                    decimal conversionRate = _rates.GetRateOf(sourceCurrency, targetCurrency);
                    if (sourceCurrency.Equals(targetCurrency))
                    {
                        return amount;
                    }

                    logger.Log(DateTime.Now, sourceCurrency, targetCurrency, conversionRate);
                    var convertedValue = amount * conversionRate;
                    return convertedValue;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
