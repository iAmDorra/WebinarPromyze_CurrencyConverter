﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using NSubstitute;

namespace CurrencyConverter.Domain.Tests
{
    [TestClass]
    public class CurrencyConverterTest
    {
        [TestMethod]
        public void Should_return_the_same_amount_when_target_currency_is_same()
        {
            string currency = "EUR";
            decimal amount = 10;
            IRates rates = Substitute.For<IRates>();
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            Converter converter = new Converter(rates, currencyVerifier);

            decimal convertedAmount = converter.Convert(amount, currency, currency);

            decimal expectedAmount = 10;
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_convert_the_amount_when_target_currency_is_different()
        {
            string currency = "EUR";
            string usdCurrency = "USD";
            decimal amount = 10;
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 1.14m;
            rates.GetRateOf(currency, usdCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            Converter converter = new Converter(rates, currencyVerifier);

            decimal convertedAmount = converter.Convert(amount, currency, usdCurrency);

            decimal expectedAmount = 11.4m;
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_convert_the_amount_when_target_currency_is_different_triangulation()
        {
            string currency = "CAD";
            string eurCurrency = "EUR";
            decimal amount = 15;
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 0.005134m;
            rates.GetRateOf(currency, eurCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            Converter converter = new Converter(rates, currencyVerifier);

            decimal convertedAmount = converter.Convert(amount, currency, eurCurrency);

            decimal expectedAmount = 0.077010m;
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }
    }
}