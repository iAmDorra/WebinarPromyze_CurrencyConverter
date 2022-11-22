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

        public Amount Convert(Amount amount, Currency sourceCurrency, Currency targetCurrency)
        {
            if (!currencyVerifier.Verify(sourceCurrency))
            {
                throw new InvalidOperationException();
            }

            if (!currencyVerifier.Verify(targetCurrency))
            {
                throw new InvalidOperationException();
            }

            if (amount.IsNegative())
            {
                throw new InvalidOperationException();
            }

            if (sourceCurrency.Equals(targetCurrency))
            {
                return amount;
            }

            decimal conversionRate = _rates.GetRateOf(sourceCurrency, targetCurrency);
            logger.Log(DateTime.Now, sourceCurrency, targetCurrency, conversionRate);
            var convertedValue = amount.MultiplyBy(conversionRate);
            return convertedValue;
        }
    }
}
