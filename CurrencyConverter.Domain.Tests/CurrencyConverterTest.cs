using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using NSubstitute;
using System;

namespace CurrencyConverter.Domain.Tests
{
    [TestClass]
    public class CurrencyConverterTest
    {
        [TestMethod]
        public void Should_return_the_same_amount_when_target_currency_is_same()
        {
            Currency currency = new Currency("EUR");
            Amount amount = new Amount(10);
            IRates rates = Substitute.For<IRates>();
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            var convertedAmount = converter.Convert(amount, currency, currency);

            var expectedAmount = new Amount(10);
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_convert_the_amount_when_target_currency_is_different()
        {
            Currency currency = new Currency("EUR");
            Currency usdCurrency = new Currency("USD");
            Amount amount = new Amount(10);
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 1.14m;
            rates.GetRateOf(currency, usdCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            currencyVerifier.Verify(usdCurrency).Returns(true);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            var convertedAmount = converter.Convert(amount, currency, usdCurrency);

            var expectedAmount = new Amount(11.4m);
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_convert_the_amount_when_target_currency_is_different_triangulation()
        {
            Currency currency = new Currency("CAD");
            Currency eurCurrency = new Currency("EUR");
            Amount amount = new Amount(15);
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 0.005134m;
            rates.GetRateOf(currency, eurCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            currencyVerifier.Verify(eurCurrency).Returns(true);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            var convertedAmount = converter.Convert(amount, currency, eurCurrency);

            var expectedAmount = new Amount(0.077010m);
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_not_convert_when_source_currency_is_invalid()
        {
            Currency currency = new Currency("EUR");
            Amount amount = new Amount(10);
            IRates rates = Substitute.For<IRates>();
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(false);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            Check.ThatCode(() => converter.Convert(amount, currency, currency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_not_convert_when_target_currency_is_invalid()
        {
            Currency currency = new Currency("CAD");
            Currency eurCurrency = new Currency("EUR");
            Amount amount = new Amount(10);
            IRates rates = Substitute.For<IRates>();
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            currencyVerifier.Verify(eurCurrency).Returns(false);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            Check.ThatCode(() => converter.Convert(amount, currency, eurCurrency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_not_convert_when_the_amount_is_negative()
        {
            Currency currency = new Currency("EUR");
            Currency usdCurrency = new Currency("USD");
            Amount amount = new Amount(-12);
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 1.14m;
            rates.GetRateOf(currency, usdCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            currencyVerifier.Verify(usdCurrency).Returns(true);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            Check.ThatCode(() => converter.Convert(amount, currency, usdCurrency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_log_every_conversion()
        {
            Currency currency = new Currency("EUR");
            Currency usdCurrency = new Currency("USD");
            Amount amount = new Amount(12);
            IRates rates = Substitute.For<IRates>();
            decimal eurUsdRate = 1.14m;
            rates.GetRateOf(currency, usdCurrency).Returns(eurUsdRate);
            ICurrencyVerifier currencyVerifier = Substitute.For<ICurrencyVerifier>();
            currencyVerifier.Verify(currency).Returns(true);
            currencyVerifier.Verify(usdCurrency).Returns(true);
            var logger = Substitute.For<ILogger>();
            Converter converter = new Converter(rates, currencyVerifier, logger);

            converter.Convert(amount, currency, usdCurrency);

            logger.Received()
                .Log(Arg.Any<DateTime>(), currency, usdCurrency, eurUsdRate);
        }
    }
}
