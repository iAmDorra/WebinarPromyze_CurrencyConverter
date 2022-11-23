using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using NSubstitute;
using System;

namespace CurrencyConverter.Domain.Tests
{
    [TestClass]
    public class CurrencyConverterTest
    {
        class ConverterBuilder
        {
            private IRates rates;
            private ICurrencyVerifier currencyVerifier;
            private ILogger logger;

            public ILogger Logger { get => logger; }

            internal ConverterBuilder WithRates(params Tuple<Currency, Currency, decimal>[] rateCollection)
            {
                rates = Substitute.For<IRates>();
                foreach (var rate in rateCollection)
                {
                    rates.GetRateOf(rate.Item1, rate.Item2).Returns(rate.Item3);
                }
                return this;
            }

            internal ConverterBuilder WithVerifier(params Tuple<Currency, bool>[] currencyExists)
            {
                currencyVerifier = Substitute.For<ICurrencyVerifier>();
                foreach (var currencyExist in currencyExists)
                {
                    currencyVerifier.Verify(currencyExist.Item1).Returns(currencyExist.Item2);
                }

                return this;
            }

            internal ConverterBuilder WithLogger()
            {
                logger = Substitute.For<ILogger>();
                return this;
            }

            internal Converter Build()
            {
                return new Converter(rates, currencyVerifier, logger);
            }
        }

        [TestMethod]
        public void Should_return_the_same_amount_when_target_currency_is_same()
        {
            Currency currency = new Currency("EUR");
            Amount amount = new Amount(10); ;
            Converter converter = new ConverterBuilder().WithRates()
                .WithVerifier(new Tuple<Currency, bool>(currency, true))
                .WithLogger()
                .Build();

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
            decimal eurUsdRate = 1.14m;
            Converter converter = new ConverterBuilder()
                .WithRates(new Tuple<Currency, Currency, decimal>(currency, usdCurrency, eurUsdRate))
                .WithVerifier(new Tuple<Currency, bool>(currency, true),
                new Tuple<Currency, bool>(usdCurrency, true))
                .WithLogger()
                .Build();

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
            decimal eurUsdRate = 0.005134m;
            Converter converter = new ConverterBuilder()
                .WithRates(new Tuple<Currency, Currency, decimal>(currency, eurCurrency, eurUsdRate))
                .WithVerifier(new Tuple<Currency, bool>(currency, true),
                new Tuple<Currency, bool>(eurCurrency, true))
                .WithLogger()
                .Build();

            var convertedAmount = converter.Convert(amount, currency, eurCurrency);

            var expectedAmount = new Amount(0.077010m);
            Check.That(convertedAmount).IsEqualTo(expectedAmount);
        }

        [TestMethod]
        public void Should_not_convert_when_source_currency_is_invalid()
        {
            Currency currency = new Currency("EUR");
            Amount amount = new Amount(10);
            Converter converter = new ConverterBuilder().WithRates()
                .WithVerifier(new Tuple<Currency, bool>(currency, false))
                .WithLogger()
                .Build();

            Check.ThatCode(() => converter.Convert(amount, currency, currency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_not_convert_when_target_currency_is_invalid()
        {
            Currency currency = new Currency("CAD");
            Currency eurCurrency = new Currency("EUR");
            Amount amount = new Amount(10);
            Converter converter = new ConverterBuilder().WithRates()
                .WithVerifier(new Tuple<Currency, bool>(currency, true),
                new Tuple<Currency, bool>(eurCurrency, false))
                .WithLogger()
                .Build();

            Check.ThatCode(() => converter.Convert(amount, currency, eurCurrency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_not_convert_when_the_amount_is_negative()
        {
            Currency currency = new Currency("EUR");
            Currency usdCurrency = new Currency("USD");
            Amount amount = new Amount(-12);
            decimal eurUsdRate = 1.14m;
            Converter converter = new ConverterBuilder()
                .WithRates(new Tuple<Currency, Currency, decimal>(currency, usdCurrency, eurUsdRate))
                .WithVerifier(new Tuple<Currency, bool>(currency, true),
                new Tuple<Currency, bool>(usdCurrency, true))
                .WithLogger()
                .Build();

            Check.ThatCode(() => converter.Convert(amount, currency, usdCurrency))
                 .Throws<InvalidOperationException>();
        }

        [TestMethod]
        public void Should_log_every_conversion()
        {
            Currency currency = new Currency("EUR");
            Currency usdCurrency = new Currency("USD");
            Amount amount = new Amount(12);
            decimal eurUsdRate = 1.14m;
            ConverterBuilder converterBuilder = new ConverterBuilder();
            Converter converter = converterBuilder
                .WithRates(new Tuple<Currency, Currency, decimal>(currency, usdCurrency, eurUsdRate))
                .WithVerifier(new Tuple<Currency, bool>(currency, true),
                new Tuple<Currency, bool>(usdCurrency, true))
                .WithLogger()
                .Build();

            converter.Convert(amount, currency, usdCurrency);

            converterBuilder.Logger.Received()
                .Log(Arg.Any<DateTime>(), currency, usdCurrency, eurUsdRate);
        }
    }
}
