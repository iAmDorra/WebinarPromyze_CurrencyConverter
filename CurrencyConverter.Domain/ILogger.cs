using System;

namespace CurrencyConverter.Domain
{
    public interface ILogger
    {
        void Log(DateTime dateTime, string sourceCurrency, string targetCurrency, decimal conversionRate);
    }
}