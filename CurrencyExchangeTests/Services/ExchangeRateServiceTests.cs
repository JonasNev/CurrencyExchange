using CurrencyExchange.Models;
using CurrencyExchange.Services;

namespace CurrencyExchangeTests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeRateServiceTests()
        {
            _exchangeRateService = new ExchangeRateService();
        }

        [Fact]
        public void GetExchangeRate_ValidCurrencyPair_ReturnsCorrectRate()
        {
            var currencyPair = new CurrencyPair("DKK", "USD");
            var exchangeRate = _exchangeRateService.GetExchangeRate(currencyPair);

            Assert.NotNull(exchangeRate);
            Assert.Equal(0.1508M, exchangeRate.Rate);
        }

        [Fact]
        public void GetExchangeRate_SameMainAndMoneyCurrency_ReturnsRateOfOne()
        {
            var currencyPair = new CurrencyPair("DKK", "DKK");
            var exchangeRate = _exchangeRateService.GetExchangeRate(currencyPair);

            Assert.NotNull(exchangeRate);
            Assert.Equal(1.0M, exchangeRate.Rate);
        }

        [Fact]
        public void GetExchangeRate_InvalidCurrencyPair_ThrowsException()
        {
            var currencyPair = new CurrencyPair("DKK", "XYZ");

            Assert.Throws<InvalidOperationException>(() => _exchangeRateService.GetExchangeRate(currencyPair));
        }

        [Fact]
        public void GetExchangeRate_InvalidMainCurrency_ThrowsException()
        {
            var currencyPair = new CurrencyPair("XYZ", "USD");

            Assert.Throws<InvalidOperationException>(() => _exchangeRateService.GetExchangeRate(currencyPair));
        }
    }
}