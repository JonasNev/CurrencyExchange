using CurrencyExchange.Models;

namespace CurrencyExchange.Services
{
    public class CurrencyConversionService
    {
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyConversionService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public decimal Convert(CurrencyPair currencyPair, decimal amount)
        {
            var exchangeRate = _exchangeRateService.GetExchangeRate(currencyPair);
            if (exchangeRate == null)
            {
                throw new InvalidOperationException($"No exchange rate available for currency pair {currencyPair.MainCurrency}/{currencyPair.IncomingCurrency}");
            }

            if (currencyPair.MainCurrency == currencyPair.IncomingCurrency)
            {
                return amount;
            }

            return amount * exchangeRate.Rate;
        }
    }
}
