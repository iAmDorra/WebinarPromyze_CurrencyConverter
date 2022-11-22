using System;

namespace CurrencyConverter.Domain
{
    public interface ILogger
    {
        void Log(DateTime dateTime, Currency sourceCurrency, Currency targetCurrency, decimal conversionRate);
    }
}