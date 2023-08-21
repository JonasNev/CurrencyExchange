using CurrencyExchange.Models;
using CurrencyExchange.Services;
using Moq;

namespace CurrencyExchangeTests.Services
{
    public class CurrencyConversionServiceTests
    {
        private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
        private readonly CurrencyConversionService _currencyConversionService;

        public CurrencyConversionServiceTests()
        {
            _exchangeRateServiceMock = new Mock<IExchangeRateService>();
            _currencyConversionService = new CurrencyConversionService(_exchangeRateServiceMock.Object);
        }

        [Fact]
        public void Convert_ValidCurrencyPair_ReturnsConvertedAmount()
        {
            var currencyPair = new CurrencyPair("DKK", "USD");
            var exchangeRate = new ExchangeRate(currencyPair, 0.1198M);
            var amount = 100M;

            _exchangeRateServiceMock.Setup(x => x.GetExchangeRate(currencyPair)).Returns(exchangeRate);

            var convertedAmount = _currencyConversionService.Convert(currencyPair, amount);

            Assert.Equal(11.98M, convertedAmount);
        }

        [Fact]
        public void Convert_SameMainAndMoneyCurrency_ReturnsOriginalAmount()
        {
            var currencyPair = new CurrencyPair("DKK", "DKK");
            var exchangeRate = new ExchangeRate(currencyPair, 1.0M);
            var amount = 100M;

            _exchangeRateServiceMock.Setup(x => x.GetExchangeRate(currencyPair)).Returns(exchangeRate);

            var convertedAmount = _currencyConversionService.Convert(currencyPair, amount);

            Assert.Equal(100M, convertedAmount);
        }

        [Fact]
        public void Convert_InvalidCurrencyPair_ThrowsException()
        {
            var currencyPair = new CurrencyPair("DKK", "XYZ");

            _exchangeRateServiceMock.Setup(x => x.GetExchangeRate(currencyPair)).Returns((ExchangeRate?)null);

            Assert.Throws<InvalidOperationException>(() => _currencyConversionService.Convert(currencyPair, 100M));
        }
    }
}
