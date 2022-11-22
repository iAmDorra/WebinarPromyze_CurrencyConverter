﻿using CurrencyConverter.Domain;
using System;

namespace CurrencyConverter.Infrastructure
{
    public class Logger : ILogger
    {
        public void Log(DateTime dateTime, Currency sourceCurrency, Currency targetCurrency, decimal rateConversion)
        {
            Console.WriteLine("{0} : {1} -> {2} : {3}", dateTime, sourceCurrency, targetCurrency, rateConversion);
        }
    }
}
